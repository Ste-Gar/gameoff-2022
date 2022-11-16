using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.gameObject.SetActive(false);
        }
    }
}
