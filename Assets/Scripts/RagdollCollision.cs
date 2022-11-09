using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    public static event EventHandler<Collision> OnAnyRagdollCollision;


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Vehicle")) return;

        OnAnyRagdollCollision?.Invoke(this, collision);
    }
}
