using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string JUMP_BUTTON = "Jump";

    CharacterController charController;
    Transform mainCam;
    float turnSmoothVelocity;
    float verticalVelocity;
    Vector3 moveDirection;

    float groundedTimer;
    [SerializeField] float jumpBuffer = .2f;

    [SerializeField] float turnSmoothTime = .1f;

    [SerializeField] float gravity = -9.81f;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpHeight = 10;

    [SerializeField] Animator animator;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        bool playerIsGrounded = charController.isGrounded;

        float lateralInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float forwardInput = Input.GetAxisRaw(VERTICAL_AXIS);
        Vector3 movement = new Vector3(lateralInput, 0, forwardInput).normalized;

        if (movement.magnitude > 0 && playerIsGrounded)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = rotation;

            moveDirection = rotation * Vector3.forward;

            animator.SetBool("isRunning", true);
        }
        else if(playerIsGrounded)
        {
            moveDirection = Vector3.zero;

            animator.SetBool("isRunning", false);
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
