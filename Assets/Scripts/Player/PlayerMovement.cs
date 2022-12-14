using Cinemachine;
using System;
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
    bool isJumping;
    bool isFalling;
    Vector3 moveDirection;

    Vector3 initialPosition;
    Quaternion initialRotation;
    CinemachineFreeLook freeLookCam;
    Vector3 initialCamPosition;
    Quaternion initialCamRotation;

    //for smooth turning; disabled as it makes movement feel sluggish
    [SerializeField] float turnSpeed = 10f;
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

        freeLookCam = FindObjectOfType<CinemachineFreeLook>();

        GameManager.OnGameReset += ResetPlayer;
    }

    private void OnDestroy()
    {
        GameManager.OnGameReset -= ResetPlayer;
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialCamPosition = freeLookCam.transform.position;
        initialCamRotation = freeLookCam.transform.rotation;
    }

    private void Update()
    {
        bool playerIsGrounded = charController.isGrounded;
        if (playerIsGrounded)
        {
            isJumping = false;
            isFalling = false;
        }
        animator.SetBool("isGrounded", playerIsGrounded);

        float lateralInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float forwardInput = Input.GetAxisRaw(VERTICAL_AXIS);
        Vector3 movement = new Vector3(lateralInput, 0, forwardInput).normalized;

        if (movement.magnitude > 0 && (allowMidairControls || playerIsGrounded))
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCamTransform.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, turnSmoothTime); //for smooth turning; disabled as it makes movement feel sluggish
            //Quaternion rotation = Quaternion.Euler(0, angle, 0);

            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed); //better turn smoothing
            //transform.rotation = rotation;

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

            //animator.SetTrigger("jump");
            isJumping = true;
        }
        if ((isJumping && verticalVelocity < 0) || verticalVelocity < -2)
        {
            isFalling = true;
        }
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);

        moveDirection.y = verticalVelocity;
        charController.Move(Time.deltaTime * moveSpeed * moveDirection);
    }

    private void OnDisable()
    {
        ResetAnimatorParameters();
    }

    private void ResetAnimatorParameters()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type != AnimatorControllerParameterType.Bool) continue;
            animator.SetBool(param.name, false);
        }
        //        animator.Rebind();
        //        animator.Update(0f);
        isJumping = false;
        isFalling = false;
        verticalVelocity = 0;
    }

    private void ResetPlayer(object sender, EventArgs e)
    {
        ResetAnimatorParameters();
        if (initialPosition != Vector3.zero)
        {
            charController.enabled = false;
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            charController.enabled = true;
        }

        if (initialCamPosition != Vector3.zero)
        {
            freeLookCam.ForceCameraPosition(initialCamPosition, initialCamRotation);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
}
