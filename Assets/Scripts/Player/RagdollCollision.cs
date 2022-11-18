using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    public static event EventHandler<Collision> OnAnyRagdollVehicleCollision;
    public static event EventHandler OnAnyRagdollGroundCollisionStay;
    public static event EventHandler OnAnyRagdollGroundCollisionEnter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Vehicle"))
            OnAnyRagdollVehicleCollision?.Invoke(this, collision);
        else if (collision.gameObject.CompareTag("Ground"))
            OnAnyRagdollGroundCollisionEnter?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        OnAnyRagdollGroundCollisionStay?.Invoke(this, EventArgs.Empty);
    }
}
