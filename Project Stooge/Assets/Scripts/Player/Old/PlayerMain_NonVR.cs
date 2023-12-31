using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain_NonVR : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rigid;
    private MeshRenderer[] meshs;
    private GameObject nearObject;

    private bool isDamage = false;
    private bool isDead = false;

    // 인스펙터
    [Header("오브젝트 연결")]
    [SerializeField]
    private GameObject playerBody;

    [Header("설정")]
    [Range(1f, 100f)]
    public int health = 100;
    [Range(1f, 100f)]
    public int maxHealth = 100;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Item")
    //    {
    //        Item item = other.GetComponent<Item>();

    //        switch (item.getType())
    //        {
    //            case Item.Type.Heart:
    //                health += item.getValue();
    //                if (health > maxHealth)
    //                    health = maxHealth;
    //                break;
    //        }

    //        Destroy(other.gameObject);
    //    }
    //    else if (other.tag == "EnemyAttack" || other.tag == "EnemyBullet")
    //    {
    //        if (!isDamage)
    //        {
    //            BulletMain enemyBullet = other.GetComponent<BulletMain>();
    //            health -= enemyBullet.GetDamage();
    //            //Debug.Log(other.GetComponent<BulletMain>().getParent());

    //            //bool isBossAttack = other.name == "Boss Melee Area";

    //            //if (other != null && other.GetComponent<Rigidbody>() != null)
    //            if (other != null)
    //            {
    //                Vector3 reactDir = (transform.position - other.transform.position).normalized;

    //                rigid.AddForce(Vector3.up * 25f, ForceMode.Impulse);
    //                rigid.AddForce(reactDir * 10f, ForceMode.Impulse);
    //            }

    //            //StartCoroutine(OnDamage(isBossAttack));
    //            StartCoroutine(OnDamage());
    //        }
    //    }
    //}

    ////IEnumerator OnDamage(bool isBossAttack)
    //IEnumerator OnDamage()
    //{
    //    if (health <= 0)
    //        OnDie();

    //    isDamage = true;

    //    foreach (MeshRenderer mesh in meshs)
    //        mesh.material.color = Color.yellow;

    //    //if (isBossAttack)
    //    //    rigid.AddForce(transform.forward * -25, ForceMode.Impulse);

    //    yield return new WaitForSeconds(0.6f);

    //    isDamage = false;

    //    foreach (MeshRenderer mesh in meshs)
    //        mesh.material.color = Color.white;

    //    //if (isBossAttack)
    //    //    rigid.velocity = Vector3.zero;
    //}

    //void OnDie()
    //{
    //    gameObject.tag = "Respawn"; // Player 태그 갖고 있으면 Enemy 타겟팅 망가짐
    //    playerBody.layer = 10; // 슈퍼아머
    //    anim.SetTrigger("doDie");

    //    rigid.velocity = Vector3.zero;

    //    isDead = true;
    //}

    public bool GetIsHit()
    {
        return isDamage;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
