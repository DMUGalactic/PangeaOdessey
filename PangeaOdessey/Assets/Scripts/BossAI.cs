using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    [RequireComponent(typeof(Animator))]
    public class BossAI : MonoBehaviour
    {
        Vector2 _originPos = default;
        BehaviorTreeRunner _BTRunner = null;
        Transform _detectedPlayer = null;
        Animator _animator = null;
        private Vector2 lastPlayerPosition; 
        private Rigidbody2D rigid;
        private Rigidbody2D player;
        private Collider2D attackRange; // 공격 범위를 나타내는 Collider
        Vector3 scale;

        [Header("Damage")]
        [SerializeField]
        private float collisionDamage = 5f; // 보스 충돌 시 데미지
        [SerializeField]
        private float meleeAttackDamage = 10f;

        [Header("Speed")]
        [SerializeField]
        private float bossRunSpeed = 1f; // 보스 이동속도
        
        [Header("CoolTime")]
        [SerializeField]
        private float meleeAttackCoolTime = 2f; // 근접 공격 간격
        [SerializeField]
        private float fireCooldown = 3f; // 원거리 공격 간격

        [Header("Distance")]
        [SerializeField]
        private float followDistance = 15f; // 플레이어를 추적할 최대 거리
        [SerializeField]
        private float attackDistance = 3f; // 공격을 시작할 거리
        [SerializeField]
        private float rangeAttackDistance = 5f;

        [Header("# Projectile Info")]
        public GameObject projectilePrefab; // 발사체 프리팹
        public float projectileDamage = 5f; // 발사체 데미지

        [Header("Boss Pattern Info")]
        public GameObject bigFirePrefab; // 불꽃 프리펩
        private float radius = 10f;    // 원 반지름
        private float count = 45;       // 생성 프리펩 개수
        private bool missionCheck = true;

        // Check
        private float lastMeleeAttackTime;
        private float lastMeleeApplyTime = 0f;
        private float lastFireTime;
        private bool isLive = true;
        private bool isFlipping = false;
        
        // Animation & Trigger Name
        const string _ATTACK_ANIM_STATE_NAME = "Dragon_Attack";
        const string _ATTACK_ANIM_TIRGGER_NAME = "isAttacking";
        const string _FIRE_ANIM_STATE_NAME = "Dragon_Fire";
        const string _FIRE_ANIM_TIRGGER_NAME = "isFiring";
        const string _RUN_ANIM_TIRGGER_NAME = "isMoving";
        const string _BURN_ANIM_STATE_NAME = "Dragon_Burn";
        const string _BURN_ANIM_TIRGGER_NAME = "isBurning";
        const string _DEATH_ANIM_TIRGGER_NAME = "Death";

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
                                new ActionNode(CheckBossHP),
                                new ActionNode(StartBossMission)
                            }
                        ),
                        new SelectorNode
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
                                        new ActionNode(CheckRangedAttacking),
                                        new ActionNode(CheckEnemyWithinRangedAttackRange),
                                        new ActionNode(DoRangedAttack),
                                    }
                                ),
                                new SequenceNode
                                (
                                    new List<INode>()
                                    {
                                        new ActionNode(Die),
                                        new ActionNode(FlipToPlayer),
                                        new ActionNode(MoveToEnemy)
                                    }
                                )
                            }
                        )
                       
                        
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
        #region Check Boss HP 
        INode.ENodeState CheckBossHP()
        {
            // 보스 체력확인
            if(GameManager.instance.bossHealth < 90){
                return INode.ENodeState.ENS_Success;
            }
            return INode.ENodeState.ENS_Failure;
        }

        INode.ENodeState StartBossMission()
        {
            // 이미 패턴 실행 했는지 확인
            if(missionCheck)
            {
                _animator.SetTrigger(_BURN_ANIM_TIRGGER_NAME);
                StartCoroutine(CreateCirclePrefabs());
                missionCheck = false;
            }
            if(IsAnimationRunning(_BURN_ANIM_STATE_NAME))
                return INode.ENodeState.ENS_Running;

            return INode.ENodeState.ENS_Failure;
        }

        #endregion


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
            if(player != null){
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer < attackDistance && Time.time > lastMeleeAttackTime + meleeAttackCoolTime)
                {
                    
                    return INode.ENodeState.ENS_Success;
                }
                return INode.ENodeState.ENS_Failure;
            }
            
            return INode.ENodeState.ENS_Failure;
        }

        INode.ENodeState DoMeleeAttack()
        {
            if (player != null)
            {
                    _animator.SetTrigger(_ATTACK_ANIM_TIRGGER_NAME);
                    
                    lastMeleeAttackTime = Time.time;
                    return INode.ENodeState.ENS_Success;
            }

            return INode.ENodeState.ENS_Failure;
        }
        #endregion

        #region Ranged Attack Node
        INode.ENodeState CheckRangedAttacking()
        {
            if (IsAnimationRunning(_FIRE_ANIM_STATE_NAME))
            {
                return INode.ENodeState.ENS_Running;
            }

            return INode.ENodeState.ENS_Success;
        }

        INode.ENodeState CheckEnemyWithinRangedAttackRange()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > rangeAttackDistance &&  distanceToPlayer < 15f && Time.time > lastFireTime )
            {
                return INode.ENodeState.ENS_Success;
            }
            
              
            return INode.ENodeState.ENS_Failure;
        }

        INode.ENodeState DoRangedAttack()
        {
            if (player != null)
            {
                _animator.SetTrigger(_FIRE_ANIM_TIRGGER_NAME);
                //SpawnProjectile();
                lastFireTime = Time.time + fireCooldown;
                
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
            if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME) || IsAnimationRunning(_FIRE_ANIM_STATE_NAME))
            {
                return INode.ENodeState.ENS_Running;
            }
            else if ( distanceToPlayer < followDistance)
            {
                Vector2 dirVec = player.position - rigid.position;
                Vector2 nextVec = dirVec.normalized * bossRunSpeed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
                rigid.velocity = Vector2.zero;
                _animator.SetTrigger(_RUN_ANIM_TIRGGER_NAME);

                // 플레이어의 마지막 위치 갱신
                lastPlayerPosition = player.position;

                return INode.ENodeState.ENS_Running;
            }

            return INode.ENodeState.ENS_Failure;
        }
        INode.ENodeState Die()
        {
            if(GameManager.instance.bossHealth <= 0)
            {
                _animator.SetBool(_DEATH_ANIM_TIRGGER_NAME,true);
                Debug.Log("보스 죽음");
                Destroy(gameObject, 2f); // 2초 후 오브젝트 제거
            }

            return INode.ENodeState.ENS_Success;
        }
        #endregion

        #region func

        public void SpawnProjectile()
        {
            float[] angles = { 0f, 10f, -10f };
            Vector2 playerDirection = ((Vector2)player.position - (Vector2)transform.position).normalized; // 방향 벡터 계산 및 정규화

            foreach (float angle in angles)
            {
                Vector2 spawnPosition = transform.position;
                if (transform.localScale.x < 0)
                {
                    spawnPosition.x -= 3;
                }
                else
                {
                    spawnPosition.x += 3;
                }
                
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    // 각도 변환 후의 방향 벡터 계산
                    Vector2 direction = Quaternion.Euler(0, 0, angle) * playerDirection;
                    projectileScript.Initialize(direction, projectileDamage);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("Bullet"))
            {
                // 보스에게 데미지를 주는 경우
                Bullet bullet = collision.GetComponent<Bullet>();
                if (bullet != null)
                {
                    GameManager.instance.TakeBossDamage(bullet.damage);
                
                }
            }
        }
        void OnTriggerStay2D(Collider2D collision)
        {
            
            if (collision.CompareTag("Player") && IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
            {
                if(Time.time >= lastMeleeApplyTime + meleeAttackCoolTime)
                {           
                    GameManager.instance.TakeDamage(meleeAttackDamage);
                    //Debug.Log("근접 공격 판정"+ GameManager.instance.health );
                    lastMeleeApplyTime = Time.time;
                } 
            }
            
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.instance.TakeDamage(collisionDamage);
                //Debug.Log("몸박");
            }
        }


        IEnumerator CreateCirclePrefabs()
        {
            Vector2 playerPosition = transform.position; // 플레이어의 현재 위치
            float angleStep = 360f / count; // 각도 간격 계산

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep;
                float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
                float x = playerPosition.x + radius * Mathf.Cos(radian); // x좌표 계산
                float y = playerPosition.y + radius * Mathf.Sin(radian); // y좌표 계산

                Vector2 spawnPosition = new Vector2(x, y); // 생성 위치 설정
                Instantiate(bigFirePrefab, spawnPosition, Quaternion.identity); // 프리팹 생성

                yield return new WaitForSeconds(0.01f);
            }
        }
        #endregion

    
    
    
    }
    
