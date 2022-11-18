using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 50;
    [SerializeField] float maxSpeed = 50;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddRelativeForce(Vector3.forward * speed);
    }
}
