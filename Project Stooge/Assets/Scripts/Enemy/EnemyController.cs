using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();


    }
    private void Update()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);// 에너미를 목표 위치로 이동
        }
    }

}
