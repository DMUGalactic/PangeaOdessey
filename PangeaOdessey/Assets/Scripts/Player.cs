using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; // 기본 이동 속도
    public float adjustedSpeed; // 조정 속도
    public float curTime;
    public float coolTime;
    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        curTime = 0;
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (curTime <= 0)
        {
            anim.SetTrigger("atk");
            curTime = coolTime;
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
{
    speed = 3;
    float totalSpeed = EquipmentManager.Instance.GetTotalStats().speed;

    // totalSpeed가 0보다 크면, 퍼센트로 변환 (예: totalSpeed가 30이면 30%는 0.3)
    float speedMultiplier = totalSpeed > 0 ? (1 + totalSpeed / 100) : 1; // 기본 속도에 퍼센트로 추가
    adjustedSpeed = speed * speedMultiplier; // 기본 속도에 추가 속도를 더함

    // 아이템 미장착 시 속도를 3으로 고정
    if (adjustedSpeed == 0)
    {
        adjustedSpeed = 3;
    }

    Vector2 nextVec = inputVec.normalized * adjustedSpeed * Time.fixedDeltaTime;
    rigid.MovePosition(rigid.position + nextVec);
}



    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.health -= Time.deltaTime * 10;
        }
        if (!GameManager.instance.isLive) return;
    }

    public void TakeDamage(float amount)
    {
        GameManager.instance.health -= amount;
        if (GameManager.instance.health < 0) GameManager.instance.health = 0;
        Debug.Log("Health after damage: " + GameManager.instance.health);

        if (GameManager.instance.health <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }
}