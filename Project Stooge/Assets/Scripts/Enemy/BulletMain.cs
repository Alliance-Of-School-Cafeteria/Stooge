using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMain : MonoBehaviour
{

    private Transform parentObject;

    /* --------------- 인스펙터 --------------- */
    [Header("설정")]
    [Range(0f, 30f)]
    public int damage = 10;
    [SerializeField]
    private bool groundDestroy = false;
    [SerializeField]
    private bool isMelee = false;

    /* -------------- 이벤트 함수 -------------- */
    private void Start()
    {
        if (isMelee)
            return;

        StartCoroutine(BulletDestroy(6f));
    }

    IEnumerator BulletDestroy(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isMelee || !groundDestroy)
            return;

        if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject, 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isMelee)
            return;

        if (other.CompareTag("Player") || other.CompareTag("Player Attack"))
            Destroy(gameObject);
    }
    
    /* ------------- 외부참조 함수 ------------- */
    public int GetDamage()
    {
        return damage;
    }

    public void SetParent(Transform transform)
    {
        parentObject = transform;
        //Debug.Log(parentObject.name);
    }

    public Transform GetParent()
    {
        return parentObject;
    }
}