using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Collider col;

    private float swingVelo;

    private void Awake()
    {
        col = GetComponent<Collider>();

        col.enabled = false;
    }

    private void Update()
    {
        OnAttack();
    }

    private void OnAttack()
    {
        swingVelo = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;

        if (swingVelo > 2f)
        {
            Debug.Log("Attack!");
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }
}
