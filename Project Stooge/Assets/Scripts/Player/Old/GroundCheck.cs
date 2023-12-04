using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGround { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        IsGround = true;
        Debug.Log("check");
    }
}
