using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroPulling : MonoBehaviour
{
    /* -------------- 이벤트 함수 -------------- */
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            Transform player = target.GetComponentInParent<Transform>().root; // Player 최상위 오브젝트 Transform


            if (gameObject.GetComponentInParent<EnemyController>() != null) // 잡몹용
            {
                gameObject.GetComponentInParent<EnemyController>().SetTarget(player);
                Debug.Log(gameObject.GetComponentInParent<EnemyMain>().name + " -> " + player);
            }
            
            gameObject.SetActive(false);
        }

    }
}