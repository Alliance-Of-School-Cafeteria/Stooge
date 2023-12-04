using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour
{

    public enum Type { Melee, Range };

    /* ------------- 컴포넌트 변수 ------------- */
    private EnemyController enemyController;
    //private Rigidbody rigid;
    //private BoxCollider boxCollider;
    private MeshRenderer[] meshs;
    private SkinnedMeshRenderer skin;
    private Animator anim;

    /* ---------------- 피격 관련 -------------- */
    private bool isDead = false;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private AudioSource meleeenemydeadSource;
    [SerializeField]
    private AudioClip meleeenemydeadClip;
    [SerializeField]
    private AudioSource rangeenemydeadSource;
    [SerializeField]
    private AudioClip rangeenemydeadClip;

    [Header("설정")]
    public Type enemyType;
    public bool Skinned = false;
    [Range(1f, 1000f)]
    public int maxHealth = 100;
    [Range(1f, 1000f)]
    public int curHealth = 100;
    [Range(0f, 5f)]
    public float knockbackForce = 0.3f;

    /* ---------------- 이벤트 함수 --------------- */
    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        skin = GetComponentInChildren<SkinnedMeshRenderer>();
        //rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")  // 근접 공격
        {
            gameObject.layer = 10;  // 슈퍼 아머

            Vector3 reactDir = transform.position - other.transform.position;
            reactDir.y = 0f;

            enemyController.SetTarget(other.GetComponentInParent<Transform>().root); // 타겟 변경(PlayerMain이 담겨있는 오브젝트로)
            //Debug.Log(other.GetComponentInParent<Transform>().root.ToString());
            StartCoroutine("OnDamage", reactDir);
        }
    }

    /* ---------------- 피격함수 --------------- */
    IEnumerator OnDamage(Vector3 reactDir)
    {
        Debug.Log(gameObject.name + " Hit!");
        enemyController.setIsHit(true);
        anim.SetBool("isWalk", false);

        transform.position += reactDir * knockbackForce;

        if (!Skinned)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.red;
        }
        else
            skin.material.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        if (!Skinned)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white; // 몬스터의 원래 색깔로 변경
        }
        else
            skin.material.color = Color.white;

        if (curHealth <= 0)
            OnDie();
        else
            gameObject.layer = 7;  // 슈퍼 아머 해제

        enemyController.setIsHit(false);
    }

    void OnDie()
    {
        gameObject.layer = 10;  // 슈퍼 아머
        isDead = true;

        //rigid.velocity = Vector3.zero;
        enemyController.SetIsNavEnabled(false);

        anim.SetTrigger("doDie");

        Destroy(gameObject, 3); // 3초 뒤에 삭제
    }
    /* ---------------  사운드 함수  -------------- */
    private void MeleeEnemyDeadSound()
    {
        meleeenemydeadSource.PlayOneShot(meleeenemydeadClip);
    }

    private void RangeEnemyDeadSound()
    {
        rangeenemydeadSource.PlayOneShot(rangeenemydeadClip);
    }

    /* --------------- 외부참조 함수 -------------- */
    public Type GetEnemyType()
    {
        return enemyType;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}