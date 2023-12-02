using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPulling : MonoBehaviour
{
    public float aggroRange = 10f; // 인스펙터에서 조절할 수 있는 어그로 범위
    internal bool isAggro = false; // 어그로 상태를 나타내는 플래그
    internal Transform target;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("AggroPulling OnTriggerEnter called");

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < aggroRange && !isAggro)
            {
                isAggro = true;

                Enemy enemy = GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    // 어그로 상태가 설정되면 즉시 공격이 시작됩니다.
                    enemy.SetTarget(other.transform);
                    StartCoroutine("AggroAttack");
                }

                // 어그로 풀링이 완료되면 비활성화
                gameObject.SetActive(false);
            }
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
