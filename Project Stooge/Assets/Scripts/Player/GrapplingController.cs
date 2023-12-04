using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingController : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */
    private PlayerController playerController;

    /* --------------- 착탄 지점 --------------- */
    private Vector3 grapplePoint;

    /* ------------- 동작 확인 변수 ------------ */
    private bool isGrappling = false;
    private bool canGrapple = true;

    /* ---------------- 인스펙터 --------------- */
    [Header("설정")]
    [SerializeField]
    private Transform gunAnchor;
    [SerializeField]
    private LayerMask grappleMask;
    [SerializeField]
    private float maxGrappleDis = 25f;
    [SerializeField]
    private float grappleDelayTime = 0.25f;
    [SerializeField]
    private float overShootYAxis = 2f;

    [Header("쿨타임")]
    [SerializeField, Range(0f, 10f)]
    private float coolTime = 1f;

    /* -------------- 이벤트 함수 -------------- */
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.1f)
            StartGrapple();
    }

    /* --------------- 기능 함수 --------------- */
    private void StartGrapple()
    {
        if (!canGrapple)
            return;

        StartCoroutine(GrappleCoolDown(coolTime));
        canGrapple = false;
        isGrappling = true;
        playerController.freeze = true;

        if (Physics.Raycast(gunAnchor.position, gunAnchor.forward, out RaycastHit hit, maxGrappleDis, grappleMask))
        {
            grapplePoint = hit.point;
            StartCoroutine(ShotGrapple(grappleDelayTime));
        }
        else
        {
            grapplePoint = gunAnchor.position + gunAnchor.forward * maxGrappleDis;
            StartCoroutine(StopGrapple(grappleDelayTime));
        }

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private IEnumerator ShotGrapple(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("shot");
        playerController.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overShootYAxis;

        playerController.JumpToPosition(grapplePoint, highestPointOnArc);

        StartCoroutine(StopGrapple(1f));
    }

    public IEnumerator StopGrapple(float time)
    {
        yield return new WaitForSeconds(time);
        playerController.freeze = false;
        isGrappling = false;
        //lr.enabled = false;
    }

    public IEnumerator GrappleCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        canGrapple = true;
    }

    /* ------------ 외부 호출 함수 ------------- */
    public bool IsGrappling()
    {
        return isGrappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    public Transform GunAnchor()
    {
        return gunAnchor;
    }
}
