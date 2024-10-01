using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts{
    [RequireComponent(typeof(Animator))]
    public class BossAI : MonoBehaviour
    {
        [Header("Range")]
        [SerializeField]
        float _detectRange = 10f;
        [SerializeField]
        float _meleeAttackRange = 3f;
        

        [Header("Movement")]
        [SerializeField]
        float _movementSpeed = 10f;

        Vector2 _originPos = default;
        BehaviorTreeRunner _BTRunner = null;
        Transform _detectedPlayer = null;
        Animator _animator = null;

        public float followDistance = 10f; // 플레이어를 추적할 최대 거리
        public float attackDistance = 3f; // 공격을 시작할 거리
        public float runSpeed = 1f;
        public float attackCooldown = 3f; // 공격 간격
        public LayerMask playerLayer; // 플레이어 레이어 마스크
        public float detectionRadius = 5f; // 탐지 반경
        public float health = 100f; // 보스의 체력
        public float attackDamage = 10f; // 공격 시 데미지
        public float collisionDamage = 5f; // 충돌 시 데미지

        [Header("# Projectile Info")]
        public GameObject projectilePrefab; // 발사체 프리팹
        public float projectileSpawnInterval = 2f; // 발사체 생성 간격
        public float projectileSpawnDistance = 10f; // 발사체 생성 거리
        public float projectileDamage = 5f; // 발사체 데미지

        private Animator animator;
        private float lastAttackTime;
        private float lastProjectileSpawnTime;
        private bool facingRight = true;
        private bool isLive = true;
        private Rigidbody2D rigid;
        private Rigidbody2D player;
        private Vector2 lastPlayerPosition;
        private Collider2D attackRange; // 공격 범위를 나타내는 Collider
       
        Vector3 scale;


        public Monster Monster;
        private bool isFlipping = false;

        const string _ATTACK_ANIM_STATE_NAME = "Attack";
        const string _ATTACK_ANIM_TIRGGER_NAME = "Attack";
        const string _RUN_ANIM_STATE_NAME = "Runing";
        const string _RUN_ANIM_TIRGGER_NAME = "Running";

        void Awake()
        {
            _animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            attackRange = GetComponentInChildren<PolygonCollider2D>();
            scale = transform.localScale;
            
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            _BTRunner = new BehaviorTreeRunner(SettingBT());
            
        }

        
        void Update()
        {
            _BTRunner.Operate();
        }
        INode SettingBT()
        {
            return new SelectorNode
                (
                    new List<INode>()
                    {
                        new SequenceNode
                        (
                            new List<INode>()
                            {
                                new ActionNode(CheckMeleeAttacking),
                                new ActionNode(CheckEnemyWithinMeleeAttackRange),
                                new ActionNode(DoMeleeAttack),
                            }
                        ),
                        new SequenceNode
                        (
                            new List<INode>()
                            {
                                //new ActionNode(CheckRangedAttacking),
                                //new ActionNode(CheckEnemyWithinRangedAttackRange),
                                //new ActionNode(DoRangedAttack),
                            }
                        ),
                        new SequenceNode
                        (
                            new List<INode>()
                            {
                                new ActionNode(FlipToPlayer),
                                new ActionNode(MoveToEnemy),
                            }
                        ),
                        
                    }
                );
        }

        // 시퀀스 상황에서 애니메이션 실행 후 다음 노드 실행
        bool IsAnimationRunning(String stateName)
        {
            if(_animator != null)
            {
                if(_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                {
                    var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                    return normalizedTime != 0 && normalizedTime < 1f;
                }
            }

            return false;
        }

        #region Melee Attack Node
        INode.ENodeState CheckMeleeAttacking()
        {
            if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
            {
                attackRange.enabled = true;
                return INode.ENodeState.ENS_Running;
            }
            attackRange.enabled = false;
            return INode.ENodeState.ENS_Success;
        }

        INode.ENodeState CheckEnemyWithinMeleeAttackRange()
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer < attackDistance && Time.time > lastAttackTime + attackCooldown)
                {
                    return INode.ENodeState.ENS_Success;
                }
            }

            return INode.ENodeState.ENS_Failure;
        }

        INode.ENodeState DoMeleeAttack()
        {
            if (player != null)
            {
                    _animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;
                    return INode.ENodeState.ENS_Success;
            }

            return INode.ENodeState.ENS_Failure;
        }
        #endregion

        #region Ranged Attack Node
        INode.ENodeState CheckRangedAttacking()
        {
            if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
            {
                return INode.ENodeState.ENS_Running;
            }

            return INode.ENodeState.ENS_Success;
        }

        INode.ENodeState CheckEnemyWithinRangedAttackRange()
        {
            if (_detectedPlayer != null)
            {
                if (Vector2.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
                {
                    return INode.ENodeState.ENS_Success;
                }
            }

            return INode.ENodeState.ENS_Failure;
        }

        INode.ENodeState DoRangedAttack()
        {
            if (_detectedPlayer != null)
            {
                _animator.SetTrigger(_ATTACK_ANIM_TIRGGER_NAME);
                return INode.ENodeState.ENS_Success;
            }

            return INode.ENodeState.ENS_Failure;
        }
        #endregion

        #region Flip & Move Node
        INode.ENodeState FlipToPlayer()
        {
            if (isFlipping) // 이미 회전 중이라면 함수 종료
            {
                return INode.ENodeState.ENS_Running;
            }

            float directionToPlayer = player.position.x - transform.position.x;

            // 현재 방향과 목표 방향이 다를 때만 회전
            if ((directionToPlayer > 0 && transform.localScale.x == -1) || (directionToPlayer < 0 && transform.localScale.x == 1))
            {
                var scale = transform.localScale;
                scale.x *= -1;   
                transform.localScale = scale;
                //Debug.Log(facingRight ? "오른쪽 회전" : "왼쪽 회전");
                isFlipping = false;
                return INode.ENodeState.ENS_Running;
            }

            return INode.ENodeState.ENS_Success;
        }

        INode.ENodeState MoveToEnemy()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);          
            
            if ( distanceToPlayer < followDistance)
            {
                Vector2 dirVec = player.position - rigid.position;
                Vector2 nextVec = dirVec.normalized * runSpeed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
                rigid.velocity = Vector2.zero;
                _animator.SetTrigger("Running");

                // 플레이어의 마지막 위치 갱신
                lastPlayerPosition = player.position;

                return INode.ENodeState.ENS_Running;
            }

            return INode.ENodeState.ENS_Failure;
        }
        #endregion

        #region func
        public void ApplyAttackDamage()
        {
            if (attackRange == null) return; // attackRange가 null인 경우 함수 종료

            Collider2D[] hits = Physics2D.OverlapBoxAll(attackRange.bounds.center, attackRange.bounds.size, 0, playerLayer);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    GameManager.instance.TakeDamage(attackDamage);
                }
            }
        }
        public void TakeDamage(float amount)
        {
            if (!isLive) return;

            health -= amount;
            //GameManager.instance.UpdateBossHealth(health);

            if (health <= 0)
            {
                Die();
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && IsAnimationRunning("Attack"))
            {                
                GameManager.instance.TakeDamage(GameManager.instance.bossDamageAmount);
                Debug.Log("근접 공격 판정");
            }
            else if (collision.CompareTag("Bullet"))
            {
                // 보스에게 데미지를 주는 경우
                Bullet bullet = collision.GetComponent<Bullet>();
                if (bullet != null)
                {
                    GameManager.instance.TakeBossDamage(bullet.damage);
                
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.instance.TakeDamage(collisionDamage);
                Debug.Log("몸박");
            }
        }

        void Die()
        {
            isLive = false;
            animator.SetTrigger("Dead");
            // 보스가 죽은 후 처리할 로직 추가 (예: 일정 시간 후 제거)
            Destroy(gameObject, 2f); // 2초 후 오브젝트 제거
        }
        #endregion

    }
    
    
}