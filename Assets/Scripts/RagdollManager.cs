using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void Update()
    {
        
    }
}
