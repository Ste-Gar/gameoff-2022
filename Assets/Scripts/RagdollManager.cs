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

    CapsuleCollider playerCollider;
    CharacterController characterController;
    PlayerMovement playerMovement;
    [SerializeField] Animator animator;

    [SerializeField] float collisionForceMulti = 100;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Z))
        //    EnableRagdoll(new Vector3(0, 10000, 0));
    }

    private void EnableRagdoll(Vector3 force)
    {
        state = RagdollState.enabled;

        characterController.enabled = false;
        playerMovement.enabled = false;
        animator.enabled = false;
        playerCollider.enabled = false;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            //rb.AddExplosionForce(300f, hitPoint, .1f, 0, ForceMode.Impulse);
            rb.AddForce(force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            //Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            Vector3 vehicleVelocity = other.attachedRigidbody.velocity;
            Vector3 playerVelocity = characterController.velocity;
            Vector3 relativeVelocity = vehicleVelocity + playerVelocity + Vector3.up * 10;

            Vector3 hitForce = relativeVelocity * collisionForceMulti;

            EnableRagdoll(hitForce);
        }    
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Debug.Log(collision.gameObject.name);
    //    if (collision.gameObject.CompareTag("Vehicle"))
    //    {
    //        Vector3 relativeVelocity = collision.relativeVelocity;


    //        EnableRagdoll(collision.relativeVelocity);
    //    }
    //}
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(hit.gameObject.name);
    //    if (hit.gameObject.CompareTag("Vehicle"))
    //    {
    //        Vector3 vehicleVelocity = hit.rigidbody.velocity;
    //        Vector3 characterVelocity = hit.controller.velocity;
    //        Vector3 hitVelocity = vehicleVelocity + characterVelocity;

    //        //Vector3 hitPoint = hit.point;
    //        Vector3 hitDirection = (hit.point - transform.position).normalized;
    //        Vector3 hitForce = Vector3.Scale(hitVelocity, hitDirection) * 30000;



    //        EnableRagdoll(hitForce);
    //    }
    //}
}
