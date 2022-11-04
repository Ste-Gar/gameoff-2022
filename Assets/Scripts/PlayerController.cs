using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string JUMP = "Jump";

    Rigidbody rb;
    float forwardInput;
    float lateralInput;
    Vector3 movementDirection;
    CharacterController charController;
    Transform mainCam;
    float turnSmoothVelocity;
    float verticalVelocity;
    Vector3 moveDirection;

    float groundedTimer;
    [SerializeField] float jumpBuffer = .2f;

    [SerializeField] float turnSmoothTime = .1f;

    [SerializeField] float gravity = -9.81f;
    //[SerializeField] float gravityScale = 10;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpHeight = 10;
    //[SerializeField] ForceMode forceMode = ForceMode.Acceleration;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        bool playerIsGrounded = charController.isGrounded;

        float forwardInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float lateralInput = Input.GetAxisRaw(VERTICAL_AXIS);
        Vector3 movement = new Vector3(forwardInput, 0, lateralInput).normalized;
        //Vector3 moveDirection;

        if (movement.magnitude > 0 && playerIsGrounded)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = rotation;

            moveDirection = rotation * Vector3.forward;
        }
        else if(playerIsGrounded)
        {
            moveDirection = Vector3.zero;
        }

        if (playerIsGrounded)
            groundedTimer = jumpBuffer;

        if (groundedTimer > 0)
            groundedTimer -= Time.deltaTime;

        if (playerIsGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }

        verticalVelocity += gravity * Time.deltaTime;

        //if (Input.GetButtonDown(JUMP) && playerIsGrounded)
        if (Input.GetButtonDown(JUMP) && groundedTimer > 0)
        {
            groundedTimer = 0;

            verticalVelocity += Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //if(!charController.isGrounded)
            //moveDirection.y += gravity;

        moveDirection.y = verticalVelocity;
        charController.Move(Time.deltaTime * moveSpeed * moveDirection);
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
