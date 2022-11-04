using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    Rigidbody rb;
    float forwardInput;
    float lateralInput;
    Vector3 movementDirection;
    CharacterController charController;
    Transform mainCam;
    float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = .1f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float gravityScale = 10;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] ForceMode forceMode = ForceMode.Acceleration;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        float forwardInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float lateralInput = Input.GetAxisRaw(VERTICAL_AXIS);
        Vector3 movement = new Vector3(forwardInput, 0, lateralInput);//.normalized;
        Vector3 moveDirection;

        if (movement.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = rotation;

            moveDirection = rotation * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        moveDirection.y += gravity;
        charController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
    }

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    mainCam = Camera.main.transform;
    //}

    //void Update()
    //{
    //    forwardInput = Input.GetAxis(HORIZONTAL_AXIS);
    //    lateralInput = Input.GetAxis(VERTICAL_AXIS);
    //    Vector3 movementInput = new Vector3(forwardInput, 0, lateralInput);

    //    float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
    //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //    Quaternion rotation = Quaternion.Euler(0, angle, 0);
    //    transform.rotation = rotation;
    //    movementDirection = rotation * Vector3.forward;
    //    Debug.Log(movementDirection);
    //}

    //private void FixedUpdate()
    //{
    //    Vector3 movement = new Vector3(forwardInput, 0, lateralInput) * moveSpeed;
    //    rb.AddForce(movement, forceMode);
    //}
}
