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

    private void Awake()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    void Start()
    {
        ragdollRigidbodies = ragdollManager.RagdollRigidbodies;

        mainCamTransform = Camera.main.transform;
    }

    void Update()
    {
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

        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(moveDirection * movementForceMulti, movementForceMode);
        }
    }
}