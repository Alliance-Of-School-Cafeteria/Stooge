using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    /* ------------- 컴포넌트 변수 ------------- */

    /* ------------ 피격 저장 변수 ------------- */
    private bool isDamage = false;
    private bool isDead = false;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private AudioSource playerdeadSource;
    [SerializeField]
    private AudioClip playerdeadClip;
    [SerializeField]
    private GameObject hitEffect;

    [Header("설정")]
    [SerializeField, Range(1f, 5f)]
    public int health = 5;
    [SerializeField, Range(1f, 5f)]
    public int maxHealth = 5;

    /* -------------- 이벤트 함수 -------------- */
    void Awake()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Attack"))
        {
            if (!isDamage)
            {
                StartCoroutine(OnDamage());
            }
        }
    }

    /* --------------- 기능 함수 --------------- */
    private IEnumerator OnDamage()
    {
        health -= 1;

        if (health <= 0)
            OnDie();

        StartCoroutine(HPCharge(10f));
        isDamage = true;

        //foreach (MeshRenderer mesh in meshs)
        //    mesh.material.color = Color.yellow;

        yield return new WaitForSeconds(0.6f);

        isDamage = false;

        //foreach (MeshRenderer mesh in meshs)
        //    mesh.material.color = Color.white;
    }

    private void OnDie()
    {
        //gameObject.tag = "Respawn";
        //playerBody.layer = 10; // 슈퍼아머
        PlayerDeadSound();
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HPCharge(float second) // hp 충전
    {
        yield return new WaitForSeconds(second);
        health += 1;
    }

    private void PlayerDeadSound()
    {
        playerdeadSource.PlayOneShot(playerdeadClip);
    }

    public bool GetIsHit()
    {
        return isDamage;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
