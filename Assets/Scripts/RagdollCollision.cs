using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    public static event EventHandler<Collision> OnAnyRagdollVehicleCollision;
    public static event EventHandler OnAnyRagdollGroundCollisionStay;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Vehicle")) return;

        OnAnyRagdollVehicleCollision?.Invoke(this, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        OnAnyRagdollGroundCollisionStay?.Invoke(this, EventArgs.Empty);
    }
}
