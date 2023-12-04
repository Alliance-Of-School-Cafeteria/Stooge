using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyHitTrigger : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */
    private Material material;

    /* -------------- 이벤트 함수 -------------- */
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player Attack"))
            StartCoroutine(Hit());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Attack"))
            StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        material.color = Color.red;
        
        yield return new WaitForSeconds(0.5f);

        material.color = Color.white;
    }
}
