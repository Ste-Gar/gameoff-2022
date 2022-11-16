using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 50;
    [SerializeField] float maxSpeed = 50;
    [SerializeField] float scoreMultiplier = 1;
    public float ScoreMultiplier { get { return scoreMultiplier; } }

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
