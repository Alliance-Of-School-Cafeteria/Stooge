using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */
    private Collider col;
    private TrailRenderer trail;

    /* ----------- 스윙 값 저장 변수 ----------- */
    private float swingVelo;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private AudioSource playerattackSource;
    [SerializeField]
    private AudioClip playerattackClip;
    
    

    [Header("설정")]
    [SerializeField, Range(0f, 10f)]
    private float needVelo = 3f;

    /* -------------- 이벤트 함수 -------------- */
    private void Awake()
    {
        col = GetComponent<Collider>();
        trail = GetComponentInChildren<TrailRenderer>();

        col.enabled = false;
        trail.enabled = false;
    }

    private void Update()
    {
        OnAttack();
    }

    private void OnAttack()
    {
        swingVelo = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
        PlayerAttackSound();
        if (swingVelo > needVelo)
        {
            StopAllCoroutines();
            Debug.Log("Attack!");
            col.enabled = true;
            trail.enabled = true;

        }
        else
        {
            col.enabled = false;
            StartCoroutine(TrailEnd());
        }
    }

    private IEnumerator TrailEnd()
    {
        yield return new WaitForSeconds(0.2f);
        trail.enabled = false;
    }

    private void PlayerAttackSound()
    {
        playerattackSource.PlayOneShot(playerattackClip);
    }
}
