using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;

    public Transform target;

//    NavMeshAgent nav;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
//        nav = GetComponent<>(NavMeshAgent);

    }
    void Update()
    {
//        nav.SetDestination(target.position);

    }

    
//    void OnTriggerEnter(Collider other)
//    {
//        //플레이어의 공격을 받을시
//        if(other.tag == "Attack")
//        {
//            Weapon weapon = other.GetComponent<Weapon>();
//            curHealth -= weapon.damage; // 현재 체력 감소
//            Vector3 reactVec = transform.position - other.transform.position; // 받은 데미지방향값 저장
//            StartCoroutine(OnDamage(reactVec));
//        }
//        else if (other.tag == "Bullet")
//        {
//            Bullet bullet = other.GetComponent<bullet>();
//            curHealth -= bullet.damage; //현재 체력 감소
//            Vector3 reactVec = transform.position - other.transform.position; // 받은 데미지방향값 저장
//            StartCoroutine(OnDamage(reactVec));
//       }
//    }
//
//    IEnumerator OnDamage(Vector3 reactVec)
//    {
//        mat.color = Color.red; // 피격판정 효과
//        yield return new WaitForSeconds(0.1f); // 0.1초 쉬었다가
//
//        if(curHealth > 0) // 현재 체력이 0보다 클시
//        {
//            mat.color = Color.null; // 피격판정시 색변환
//            /* 벡터값 설정 */
//            reactVec = reactVec.nomalized;
//            reactVec += Vector3.back;
//            //뒤로 밀려남
//            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
//        }
//        else
//        {
//            mat.color = Color.//원래 색으로
//            gameObject.layer = 7;
//            Destroy(gameObject, 2); // 현재 체력이 0보다 작을시 사망
//        }
//    }
}

