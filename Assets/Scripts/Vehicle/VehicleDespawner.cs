using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            GameObject vehicleGO = other.transform.parent.gameObject;
            other.attachedRigidbody.velocity = Vector3.zero;
            vehicleGO.SetActive(false);
        }
    }
}
