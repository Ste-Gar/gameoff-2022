using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string JUMP_BUTTON = "Jump";

    CharacterController charController;
    Transform mainCamTransform;
    float verticalVelocity;
    Vector3 moveDirection;

    //for smooth turning; disabled as it makes movement feel sluggish
    //float turnSmoothVelocity;
    //[SerializeField] float turnSmoothTime = .1f;

    float groundedTimer;
    [SerializeField] float jumpBuffer = .2f;

    [SerializeField] float gravity = -9.81f;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] bool allowMidairControls;

    [SerializeField] Animator animator;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        mainCamTransform = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        bool playerIsGrounded = charController.isGrounded;
        animator.SetBool("isGrounded", playerIsGrounded);

        float lateralInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float forwardInput = Input.GetAxisRaw(VERTICAL_AXIS);
        Vector3 movement = new Vector3(lateralInput, 0, forwardInput).normalized;

        if (movement.magnitude > 0 && (allowMidairControls || playerIsGrounded))
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCamTransform.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //for smooth turning; disabled as it makes movement feel sluggish
            //Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = rotation;

            moveDirection = rotation * Vector3.forward;

            animator.SetBool("isRunning", true);
        }
        else if (playerIsGrounded || allowMidairControls)
        {
            moveDirection = Vector3.zero;

            animator.SetBool("isRunning", false);
        }

        if (playerIsGrounded)
        {
            groundedTimer = jumpBuffer;
        }

        if (groundedTimer > 0)
            groundedTimer -= Time.deltaTime;

        if (playerIsGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }

        verticalVelocity += gravity * Time.deltaTime;

        if (Input.GetButtonDown(JUMP_BUTTON) && groundedTimer > 0)
        {
            groundedTimer = 0;

            verticalVelocity += Mathf.Sqrt(jumpHeight * -2 * gravity);

            animator.SetTrigger("jump");
        }

        moveDirection.y = verticalVelocity;
        charController.Move(Time.deltaTime * moveSpeed * moveDirection);
    }
}