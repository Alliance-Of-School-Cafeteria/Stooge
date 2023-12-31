using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */
    private EnemyMain enemyMain;
    //private Rigidbody rigid;
    private NavMeshAgent nav;
    private Animator anim;
    private Transform target;

    /* --------------- 추격 관련 --------------- */
    private bool isChase = false;
    private bool isHit = false;
    private bool isAttack = false;
    private bool isAggro = false;

    /* ---------------- AI 변수 ---------------- */
    private int moveDir = 0;
    private int isMove = 0;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private BoxCollider meleeArea;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject agrroPulling;
    [SerializeField]
    private Transform anchorPoint;

    /* ---------------사운드 관련 -------------- */
    [SerializeField]
    private AudioSource meleeenemyattackSource;
    [SerializeField]
    private AudioClip meleeenemyattackClip;
    [SerializeField]
    private AudioSource rangeenemyattackSource;
    [SerializeField]
    private AudioClip rangeenemyattackClip;
    [SerializeField]
    private AudioSource rangeenemymoveSource;
    [SerializeField]
    private AudioClip rangeenemymoveClip;

    [Header("설정")]
    public float moveDis = 3f;
    public float moveSpeed = 5f;
    public bool attackCancel = true;

    /* -------------- 이벤트 함수 -------------- */
    void Awake()
    {
        enemyMain = GetComponent<EnemyMain>();
        //rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        StartCoroutine("Think", (Random.Range(0.5f, 4f))); // 논어그로
    }

    void FixedUpdate()
    {
        if (enemyMain.GetIsDead())
            return;

        if (!isAggro) // 논어그로
        {
            EnemyMove();
        }
        else // 어그로 풀링
        {
            //FreezeVelocity();
            TargetisAlive();
            EnemyChase();
            Aiming();
            AttackCancel();
        }
    }

    /* --------------- 타겟 관련 --------------- */
    public void SetTarget(Transform transform) // 타겟 (재)설정
    {
        target = transform;
        isAggro = true;

        SetIsNavEnabled(true);

        Debug.Log(gameObject.name + " target reset");
        StopCoroutine("Think");
        StartCoroutine(ChaseStart());
    }

    void TargetOff() // 타겟 해제
    {
        //rigid.velocity = Vector3.zero;

        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);

        SetIsNavEnabled(false);
        agrroPulling.SetActive(true);
        isAggro = false;

        StartCoroutine("Think", (Random.Range(0.5f, 4f)));
    }

    /* --------------- 논 어그로 --------------- */
    void EnemyMove() // 어그로 아닐 때 이동
    {
        transform.position += transform.forward * moveSpeed * isMove * Time.deltaTime;
        //rigid.velocity = transform.forward * moveSpeed * isMove; // moveSpeed는 지정, isMove는 Think()에서 결정
    }

    IEnumerator Think(float worry) // 어그로 아닐 때 이동 결정하는 함수
    {
        yield return new WaitForSeconds(worry);     // 고민
        moveDir = Random.Range(0, 360);             // 랜덤 방향 이동
        transform.Rotate(0, moveDir, 0);
        isMove = 1;
        anim.SetBool("isWalk", true);

        yield return new WaitForSeconds(moveDis);   // 일정 거리 까지
        isMove = 0;                                 // 멈춤
        anim.SetBool("isWalk", false);

        yield return new WaitForSeconds(worry);     // 고민
        moveDir = -180;                             // 되돌아감
        transform.Rotate(0, moveDir, 0);
        isMove = 1;
        anim.SetBool("isWalk", true);

        yield return new WaitForSeconds(moveDis);   // 일정 거리 까지
        isMove = 0;                                 // 멈춤
        anim.SetBool("isWalk", false);

        StartCoroutine("Think", (Random.Range(0.5f, 4f)));
    }

    /* -------------- 어그로 풀링 --------------- */
    IEnumerator ChaseStart()
    {
        yield return new WaitForSeconds(0.8f);
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    //void FreezeVelocity()
    //{
    //    if (isChase)
    //    {
    //        rigid.velocity = Vector3.zero;
    //        rigid.angularVelocity = Vector3.zero;
    //    }
    //}

    void TargetisAlive() // 타겟 죽는거 확인
    {
        //Debug.Log(target.ToString());
        if (target == null) // 타겟이 없으면
        {
            TargetOff();
            return;
        }
        //else if (target.gameObject.GetComponent<PlayerMain>().GetIsDead()) // 타겟이 죽으면
        //{
        //    TargetOff();
        //    return;
        //}
        else // 타겟이 있고 살아 있다면
            return;

        //Debug.Log(target.parent.gameObject.ToString());
    }

    /* ---------------- 추격 관련 --------------- */
    void EnemyChase()
    {
        if (!nav.enabled)
            return;

        nav.SetDestination(target.position);
        nav.isStopped = !isChase || isHit;
    }

    void Aiming() // 레이캐스트로 플레이어 위치 특정
    {
        float targetRadius = 0;
        float targetRange = 0;

        switch (enemyMain.GetEnemyType())
        {
            case EnemyMain.Type.Melee:
                targetRadius = 1f;
                targetRange = 0.5f;
                break;

            case EnemyMain.Type.Range:
                targetRadius = 0.5f;
                targetRange = 25f;
                break;
        }

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,           // 위치
                                  targetRadius,                 // 반지름
                                  transform.forward,            // 방향
                                  targetRange,                  // 방향으로 부터 거리
                                  LayerMask.GetMask("Player")); // 레이어 특정

        if (rayHits.Length > 0 && !isAttack && !isHit)
            StartCoroutine("Attack");
    }
    /* --------------- 공격 관련 --------------- */
    IEnumerator Attack()
    {
        Debug.Log("Deal");
        isChase = false;
        isAttack = true;

        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isAttack", true);

        MeleeEnemyAttackSound();

        switch (enemyMain.GetEnemyType())
        {
            case EnemyMain.Type.Melee:
                yield return new WaitForSeconds(0.5f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.6f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(0.5f);
                break;

            case EnemyMain.Type.Range:
                yield return new WaitForSeconds(0.8f);
                GameObject instantBullet = Instantiate(bullet, new Vector3(anchorPoint.position.x, anchorPoint.position.y, anchorPoint.position.z), anchorPoint.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                RangeEnemyAttackSound();
                instantBullet.GetComponent<BulletMain>().SetParent(transform); // Buller에 발사한 객체 정보 저장

                yield return new WaitForSeconds(2f);
                break;

        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void AttackCancel()
    {
        if (!attackCancel)
            return;

        if (isHit)
        {
            StopCoroutine("Attack");

            isChase = true;
            isAttack = false;

            anim.SetBool("isAttack", false);

            if (meleeArea != null)
                meleeArea.enabled = false;
        }
    }

    private void MeleeEnemyAttackSound()
    {
        meleeenemyattackSource.PlayOneShot(meleeenemyattackClip);
    }

    private void RangeEnemyAttackSound()
    {
        rangeenemyattackSource.PlayOneShot(rangeenemyattackClip);
    }

    

    /* ------------- 외부 참조함수 -------------- */
    public void SetIsNavEnabled(bool bol)
    {
        isChase = bol;
        nav.enabled = bol;
    }

    public void SetIsChase(bool bol)
    {
        isChase = bol;
    }

    public void setIsHit(bool bol)
    {
        isHit = bol;
    }
}