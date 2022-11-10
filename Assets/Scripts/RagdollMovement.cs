using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMovement : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    RagdollManager ragdollManager;
    Rigidbody[] ragdollRigidbodies;

    Transform mainCamTransform;
    Vector3 movementInput;
    Vector3 moveDirection;

    [SerializeField] float movementForceMulti = 10f;
    [SerializeField] ForceMode movementForceMode;
    [SerializeField] float velocityDeadzone = 10f;
    [SerializeField] float groundImpactVelocityMulti = 45f;
    [SerializeField] float groundImpactInterval = 0.5f;
    float groundImpactTimer;

    private void Awake()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    private void OnEnable()
    {
        RagdollCollision.OnAnyRagdollGroundCollisionStay += OnGroundCollisionStay;
    }

    private void OnDisable()
    {
        RagdollCollision.OnAnyRagdollGroundCollisionStay -= OnGroundCollisionStay;
    }

    void Start()
    {
        ragdollRigidbodies = ragdollManager.RagdollRigidbodies;

        mainCamTransform = Camera.main.transform;
    }

    void Update()
    {
        groundImpactTimer += Time.deltaTime;

        float lateralInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float forwardInput = Input.GetAxisRaw(VERTICAL_AXIS);
        movementInput = new Vector3(lateralInput, 0, forwardInput).normalized;

        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + mainCamTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        //transform.rotation = rotation;

        moveDirection = rotation * Vector3.forward;
    }

    private void FixedUpdate()
    {
        if (movementInput == Vector3.zero) return;

        if (ragdollRigidbodies[0].velocity.sqrMagnitude < velocityDeadzone) return;
        //if (ragdollRigidbodies[0].transform.position.y < 100) return;

        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(moveDirection * movementForceMulti, movementForceMode);
        }
    }

    private void OnGroundCollisionStay(object sender, EventArgs e)
    {
        if (groundImpactTimer < groundImpactInterval) return;

        groundImpactTimer = 0;
        float velocityMultiplier = groundImpactVelocityMulti * Time.deltaTime;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.velocity *= velocityMultiplier;
        }
        Debug.Log($"VelMulti: {velocityMultiplier}");
    }
}
