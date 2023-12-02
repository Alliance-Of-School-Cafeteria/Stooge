using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        Melee,
        Range,
    };

    /* -------------- 에너미 변수 -------------- */

    private Transform target;
    private bool isChase;
    private bool isAttack;
    private bool isFirstC;
    private bool isDead;


    /* --------------   인스펙터   -------------- */
    [Header("설정")]
    [SerializeField, Range(0, 20)]
    private BoxCollider meleeArea;
    [SerializeField]
    private Type enemyType;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int curHealth;
    [SerializeField]
    private int score;
    [SerializeField]
    private float aggroRange = 10f;
    [SerializeField]
    private float wanderRadius = 10f;

    /* -------------- 선언 변수??? -------------- */
    protected Rigidbody _rigidbody;
    protected BoxCollider _boxCollider;
    protected MeshRenderer[] _meshRenderers;
    protected NavMeshAgent _nav;
    protected Animator _animator;

    /* -------------- 이동관련변수 -------------- */
    private bool _isWandering = false;
    private bool _isAggro = false;
    private AggroPulling _aggroPulling;

    /* ------------------ 이벤트------------------ */
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _aggroPulling = GetComponentInChildren<AggroPulling>();

    }

    void Update()
    {
        if (_nav.enabled)
        {
            if (isChase)
            {
                _nav.SetDestination(target.position);
                _nav.isStopped = !isChase;

                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;


                // 플레이어와의 거리를 체크하여 일정 거리 이상 멀어지면 배회 시작
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (distanceToPlayer > aggroRange)
                {
                    isChase = false;
                    isAttack = false;
                    _animator.SetBool("isAttack", false);
                    StartCoroutine(Wander());
                }
            }
            else
            {
                if (!_isWandering && !_isAggro)
                    StartCoroutine(Wander());
            }
        }
    }

    void FixedUpdate()
    {
        Targeting();
    }

    /* --------------- 기능 함수 --------------- */
    IEnumerator ChaseStart()
    {
        Debug.Log("ChaseStart called");
        yield return new WaitForSeconds(0.8f);
        isChase = true;
        _animator.SetBool("isWalk", true);
    }

    /* -------------- 배회하는 변수 -------------- */
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander Start");
        _isWandering = true;

        while (!_isAggro)
        {
            // 플레이어를 감지
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange, LayerMask.GetMask("Player"));
            if (colliders.Length > 0)
            {
                Transform playerTransform = colliders[0].transform;

                // 배회 중 어그로가 없을 때 플레이어를 탐지하면 어그로 설정
                _aggroPulling.gameObject.SetActive(true);
                _aggroPulling.isAggro = true;
                _aggroPulling.SetTarget(playerTransform);

                // 배회 중 어그로가 없을 때 플레이어를 탐지하면 공격
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                {
                    yield return new WaitForSeconds(2f);
                    enemy.SetTarget(playerTransform);
                    StartCoroutine("ChaseStart");
                    
                }

                break; // 어그로가 설정되면 루프 종료
            }

            Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            _nav.SetDestination(newPosition);
            _animator.SetBool("isWalk", true);

            yield return new WaitForSeconds(2f);
        }

        _isWandering = false;
    }

    /* -------------- 타겟 지정변수 -------------- */
    public void SetTarget(Transform newTarget)
    {
        Debug.Log("SetTarget called");
        target = newTarget;
        isChase = true;
        _animator.SetBool("isWalk", true);
        StartCoroutine("ChaseStart");
    }


    void ClearTarget()
    {
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttack", false);

        SetIsNavEnabled(false);

        if (_isAggro && _aggroPulling != null) // 어그로 풀링이 존재하는 경우에만 처리
            _aggroPulling.gameObject.SetActive(true);

        _isAggro = false;

        // 어그로가 없는 경우에만 배회
        if (!_isAggro)
            StartCoroutine(Wander());
    }

    /* -------------- 공격관련 변수 -------------- */
    void Targeting()
    {
        if (!isDead && isChase)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.Melee:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.Range:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
                StartCoroutine("Attack");
                
        }
    }

    IEnumerator Attack()
    {
        
        isChase = false;
        isAttack = true;
        _animator.SetBool("isAttack", true);
        _isWandering = false;

        switch (enemyType)
        {
            case Type.Melee:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;

            case Type.Range:
                yield return new WaitForSeconds(0.5f);
//                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
//                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
//                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack = false;
        _animator.SetBool("isAttack", false);
        

    }

    /* -------------- 피격관련 변수 -------------- */
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌했을 때의 처리
            SetTarget(other.transform);
        }

        if (other.tag == "Melee")
        {
//            curHealth -= weapon.damage;
//            Vector3 reactVec = transform.position - other.transform.position;
//            StartCoroutine(OnDamage(reactVec, false));
        }
    }


    IEnumerator OnDamage(Vector3 reactVec)
    {
        
        yield return new WaitForSeconds(0.1f);

        if ( curHealth < 0 && curHealth == 0 )
        {
            gameObject.layer = 14;
            isDead = true;
            isChase = false;
            _nav.enabled = false;
            _animator.SetTrigger("doDie");

//            Player player = target.GetComponent<Player>();

            Destroy(gameObject, 4);
        }
    }

    /* -------------- 외부참조 함수 -------------- */
    public void SetIsNavEnabled(bool bol)
    {
        isChase = bol;
        _nav.enabled = bol;
    }

    
}
