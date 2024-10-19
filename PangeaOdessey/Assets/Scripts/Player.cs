using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float speedMultiplier; // 이동 속도에 곱해질 추가 변수
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
        speedMultiplier = 1.0f; // 기본 배율은 1 (변경 가능)
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
        // speedMultiplier로 이동 속도를 10%, 20%, 30%씩 증가시키는 로직
        Vector2 nextVec = inputVec.normalized * speed * speedMultiplier * Time.fixedDeltaTime;
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