using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */
    private Animator anim;
    private Rigidbody rigid;
    private CameraController camControl;
    private PlayerMain playerMain;

    /* ----------- 입력 값 저장 변수 ----------- */
    private Vector2 moveInput;
    private bool jumpInput;
    private bool attackInput;

    /* ------------ 동작 확인 변수 ------------- */
    private bool isMove = false;
    private bool isJump = false;
    private bool isAttack = false;
    private bool isGrounded = false;

    /* ------------ 방향 저장 변수 ------------- */
    private Vector3 moveDir;
    private Vector3 saveDir;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private Transform playerBody;

    [Header("설정")]
    [SerializeField, Range(1f, 30f)]
    private float moveSpeed = 20f;
    [SerializeField, Range(1f, 100f)]
    private float jumpPower = 30f;
    [SerializeField]
    private Vector3 raySize = new Vector3(1.8f, 0.6f, 1.8f);

    /* ---------------- 프로퍼티 --------------- */
    public void SetIsGrounded(bool bol)
    {
        isGrounded = bol;
    }

    public void SetForward(Vector3 dir)
    {
        playerBody.forward = dir;
    }

    /* -------------- 이벤트 함수 -------------- */
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        camControl = GetComponentInChildren<CameraController>();
        playerMain = GetComponent<PlayerMain>();
    }

    void Update()
    {
        if (playerMain.GetIsDead())
        {
            rigid.velocity = Vector3.zero;
            return;
        }

        /* 입력 값 저장 */
        GetInput();

        /* 플레이어 조작 */
        PlayerMove();
        PlayerJump();
        PlayerAttack();

        /* 바닥 체크 */
        GroundCheck();
    }

    /* --------------- 기능 함수 --------------- */
    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 이동 입력 벡터
        jumpInput = Input.GetButtonDown("Jump");
        attackInput = Input.GetButtonDown("Fire1");
    }

    private void PlayerMove()
    {
        if (playerMain.GetIsHit()) // 회피, 피격 중 이동 제한
            return;

        isMove = moveInput.magnitude != 0; // moveInput의 길이로 입력 판정

        if (isMove)
        {
            moveDir = camControl.GetMoveDir(moveInput);
            SetForward(moveDir);
            //playerBody.forward = lookForward; // 캐릭터 고정
            //playerBody.forward = moveDir;     // 카메라 고정


            rigid.velocity = new Vector3((moveDir * moveSpeed).x, rigid.velocity.y, (moveDir * moveSpeed).z); // 물리 이동
        }
        else
            rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f); // 미끄러짐 방지

        anim.SetBool("isRun", isMove);     // true일 때 걷는 애니메이션, false일 때 대기 애니메이션
    }

    private void PlayerJump()
    {
        if (jumpInput && !isJump && !playerMain.GetIsHit() && !isAttack)
        {
            isJump = true;
            isGrounded = false;

            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    private void PlayerAttack()
    {
        if (attackInput && !isAttack && !playerMain.GetIsHit())
        {
            Debug.Log("Attack");
        }
    }

    private void GroundCheck()
    {
        if (rigid.velocity.y > 0) // 추락이 아닐 때
            return;

        if (isGrounded)
        {
            isJump = false;
            anim.SetBool("isJump", false);
            //Debug.Log("착지");
        }
    }
}
