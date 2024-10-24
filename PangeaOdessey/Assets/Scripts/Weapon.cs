using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    [SerializeField] private float baseDamage; // 무기의 기본 데미지
    private float damage; // 최종 데미지

    public int count;
    public float speed;

    float savex;
    float savey;

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();

        // 무기 초기화 시 기본 데미지와 장비로부터 얻은 데미지를 합산
        UpdateDamage();
    }

    // 무기의 데미지를 장비에 따라 갱신하는 함수
    public void UpdateDamage()
{
    float equipmentDamagePercent = 0f; // 장비에서 얻는 데미지의 비율 (퍼센트)

    if (EquipmentManager.Instance != null)
    {
        equipmentDamagePercent = EquipmentManager.Instance.GetTotalStats().damage; // 예: 10, 20, 30
        Debug.Log("EquipmentManager로부터 추가된 공격력 비율: " + equipmentDamagePercent + "%");
    }

    // equipmentDamagePercent가 퍼센트로 제공된다고 가정 (예: 10이면 0.1)
    float damageMultiplier = 1 + (equipmentDamagePercent / 100); // 1 + 0.1 => 1.1가 되어야 함

    // 기본 데미지에 장비 데미지를 곱하여 최종 데미지를 계산
    damage = baseDamage * damageMultiplier; // 1 * 1.1 = 1.1
    Debug.Log("갱신된 데미지: " + damage);
}


    void Start()
    {
        Init();
        savex = 1f;
        savey = 0f;
    }

    void Update()
    {
        // 플레이어의 움직임에 따라 좌표 갱신
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
        {
            savex = x;
            savey = y;
        }

        // 무기 종류에 따라 동작 실행
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Axe();
                }
                break;
            case 2:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Bow();
                }
                break;
            case 3:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fireball();
                }
                break;
            case 4:
                // 추가 무기 구현 필요
                break;
            default:
                break;
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                BatchShield();
                break;
            case 1:
                speed = 1f; // 무기 속도
                break;
            case 2:
                speed = 1f;
                break;
            case 3:
                speed = 1f;
                break;
            case 4:
                Waterball();
                break;
            default:
                break;
        }
    }



    void BatchShield() // ���� ��ġ�ϴ� �Լ�
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 2f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1�� ��� ����
        }
    }

    void Axe() // ���� �Լ�
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        StartCoroutine(RotateAndMove(bullet, dir));
        bullet.GetComponent<Bullet>().Init(damage, -2, dir);
    }

    IEnumerator RotateAndMove(Transform target, Vector3 dir) // ���� ȸ���ϸ� ���ư��� �ڷ�ƾ
    {
        float rotationSpeed = 180f; // 1�ʿ� 180���� ȸ���ϵ��� ����
        float angle = 0f;
        while (angle < 360f)
        {
            // �ð��� ���� ȸ�� ������ ������ŵ�ϴ�.
            float rotationAmount = rotationSpeed * Time.deltaTime;
            target.Rotate(Vector3.forward, rotationAmount);
            target.Translate(dir * 2f * Time.deltaTime, Space.World);
            angle += Mathf.Abs(rotationAmount);
            yield return null;
        }
    }

    void Bow()
    {
        if (!player.scanner.nearestTarget || count <= 0)
            return;

        Vector2 targetPos = player.scanner.nearestTarget.position;
        Vector2 dir = targetPos - (Vector2)player.transform.position;

        // ���� ���� ���� �� �߸� �߻�
        if (count == 1)
        {
            FireBullet(dir.normalized, GetAngleFromVector(dir));
        }
        else
        {
            // �߻� ���� ���
            float angleStep = 40f / (count - 1); // �� ���� ������ ȭ�� ���� ���� ����
            float startAngle = -20f; // ���� ����

            for (int index = 0; index < count; index++)
            {
                // ���� �ε����� �ش��ϴ� ���� ���
                float currentAngle = startAngle + (angleStep * index);
                Vector2 direction = RotateVector(dir.normalized, currentAngle);

                FireBullet(direction, currentAngle + GetAngleFromVector(dir));
            }
        }
    }

    void FireBullet(Vector2 direction, float angle)
    {
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = player.transform.position;
        bullet.rotation = Quaternion.Euler(0, 0, angle);
        bullet.GetComponent<Bullet>().Init(damage, 0, direction);
    }

    // 2D ���Ϳ��� ������ ��� �Լ�
    float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    // 2D ���� ȸ�� �Լ�
    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }

    void Fireball()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;

        dir = dir.normalized;
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.GetComponent<Bullet>().Init(damage, -3, dir);
    }

    void Waterball()
    {
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        bullet.localPosition = Vector3.zero;
        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
    }
}
