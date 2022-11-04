using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float forwardInput;
    float lateralInput;
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    [SerializeField] float moveSpeed = 5;
    [SerializeField] ForceMode forceMode = ForceMode.Acceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        forwardInput = Input.GetAxis(HORIZONTAL_AXIS);
        lateralInput = Input.GetAxis(VERTICAL_AXIS);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(forwardInput, 0, lateralInput) * moveSpeed;
        rb.AddForce(movement, forceMode);
    }
}
