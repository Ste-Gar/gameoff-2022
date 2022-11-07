using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    enum RagdollState
    {
        disabled,
        enabled
    }

    RagdollState state = RagdollState.disabled;
    private Rigidbody[] ragdollRigidbodies;

    CharacterController characterController;
    PlayerMovement playerMovement;
    [SerializeField] Animator animator;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
            EnableRagdoll();
    }

    private void EnableRagdoll()
    {
        state = RagdollState.enabled;

        characterController.enabled = false;
        playerMovement.enabled = false;
        animator.enabled = false;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
    }
}
