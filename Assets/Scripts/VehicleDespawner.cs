using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
