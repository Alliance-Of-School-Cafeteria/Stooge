using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGround {  get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
            IsGround = true;
        else
            IsGround = false;
    }
}
