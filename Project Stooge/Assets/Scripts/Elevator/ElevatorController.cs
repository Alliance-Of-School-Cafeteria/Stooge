using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public enum ElevatorType { Up, Down };

    /* ---------------- 인스펙터 --------------- */
    [Header("설정")]
    [SerializeField, Range(0f, 50f)]
    private float evSpeed = 5f;  // 엘리베이터의 속도

    public ElevatorType elevatorType;

    private bool isPlayerOnElevator = false;

    private void Update()
    {
        if (isPlayerOnElevator)
        {
            if (elevatorType == ElevatorType.Up)
            {
                transform.Translate(Vector3.up * evSpeed * Time.deltaTime);
            }
            else if (elevatorType == ElevatorType.Down)
            {
                transform.Translate(Vector3.down * evSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
        }
    }
}