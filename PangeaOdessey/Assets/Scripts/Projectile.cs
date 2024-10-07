using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage; // 발사체의 데미지
    public float lifeTime = 5f; // 발사체의 생명 시간

    private Vector3 moveDirection;
    private float spawnTime;

    public void Initialize(Vector3 target, float damageAmount)
    {
        // 발사체의 시작 위치 고정
        if (target.x >= transform.position.x)
        {
            transform.position += new Vector3(3, 0.5f, 0); // 오른쪽으로 3만큼 이동
        }
        else
        {
            transform.position += new Vector3(-3, 0.5f, 0); // 왼쪽으로 3만큼 이동
        }

        // 오프셋된 시작 위치에서 목표까지의 방향 계산 및 정규화
        Vector3 direction = target;
        

        // 발사체의 회전 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        damage = damageAmount;
        spawnTime = Time.time;

        // 이동 방향 설정
        moveDirection = direction;
    }

    private void Update()
    {
        // 발사체를 목표 방향으로 계속 이동
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // 일정 시간이 지나면 발사체 파괴
        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(damage);
            Destroy(gameObject); // 발사체 충돌 시 파괴
        }
    }
}