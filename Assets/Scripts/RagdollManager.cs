using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private class BoneTransform
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }

    enum RagdollState
    {
        disabled,
        enabled,
        standingUp,
        resettingBones
    }

    RagdollState state = RagdollState.disabled;
    private Rigidbody[] ragdollRigidbodies;
    public Rigidbody[] RagdollRigidbodies { get { return ragdollRigidbodies; } }
    private Transform hipsBone;

    private BoneTransform[] standUpBoneTransforms;
    private BoneTransform[] ragdollBoneTransforms;
    private Transform[] bones;
    private float bonesResetElapsedTime;

    CapsuleCollider playerCollider;
    CharacterController characterController;
    PlayerMovement playerMovement;
    [SerializeField] Animator animator;

    [SerializeField] string faceUpStandAnimationStateName;
    [SerializeField] string faceUpStandAnimationClipName;
    [SerializeField] float timeToResetBones = 0.5f;

    [SerializeField] float collisionForceMulti = 100;

    float lastCollisionTime;
    [SerializeField] float ragdollCollisionInterval = 0.3f;

    private void Awake()
    {
        RagdollCollision.OnAnyRagdollCollision += OnRagdollCollision;

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider>();

        hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);

        bones = hipsBone.GetComponentsInChildren<Transform>();
        standUpBoneTransforms = new BoneTransform[bones.Length];
        ragdollBoneTransforms = new BoneTransform[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            standUpBoneTransforms[i] = new BoneTransform();
            ragdollBoneTransforms[i] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(faceUpStandAnimationClipName, standUpBoneTransforms);
    }

    private void OnRagdollCollision(object sender, Collision e)
    {
        if (Time.time - lastCollisionTime < ragdollCollisionInterval) return;

        lastCollisionTime = Time.time;
        ThrowRagdoll(e.collider);
    }

    void Update()
    {
        switch (state)
        {
            case RagdollState.disabled:
                break;
            case RagdollState.enabled:
                RagdollBehaviour();
                break;
            case RagdollState.standingUp:
                StartCoroutine(StandingUpBehaviour());
                break;
            case RagdollState.resettingBones:
                ResettingBonesBehaviour();
                break;
        }
    }

    private void RagdollBehaviour()
    {
        //TODO: add bouncing, scoring, etc...
        //Trigger DisableRagdoll someway

        if (Input.GetButtonDown("ResetPlayer"))
        {
            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(ragdollBoneTransforms);
            state = RagdollState.resettingBones;
            bonesResetElapsedTime = 0;
        }
    }

    public void DisableRagdoll()
    {
        //characterController.enabled = true;
        //playerMovement.enabled = true;
        animator.enabled = true;
        playerCollider.enabled = true; 

        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
        characterController.enabled = false;
        playerMovement.enabled = false;
        animator.enabled = false;
        playerCollider.enabled = false;

        state = RagdollState.enabled;
    }

    private void AlignRotationToHips()
    {
        Vector3 currentHipsPosition = hipsBone.position;
        Quaternion currentHipsRotation = hipsBone.rotation;

        Vector3 desiredDirection = hipsBone.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        hipsBone.position = currentHipsPosition;
        hipsBone.rotation = currentHipsRotation;
    }

    private void AlignPositionToHips()
    {
        Vector3 currentHipsPosition = hipsBone.position;
        transform.position = hipsBone.position;

        Vector3 positionOffset = standUpBoneTransforms[0].position;
        positionOffset.y = 0;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        hipsBone.position = currentHipsPosition;
    }



    private void ResettingBonesBehaviour()
    {
        bonesResetElapsedTime += Time.deltaTime;
        float elapsedPercentage = bonesResetElapsedTime / timeToResetBones;

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localPosition = Vector3.Lerp(ragdollBoneTransforms[i].position, standUpBoneTransforms[i].position, elapsedPercentage);
            bones[i].localRotation = Quaternion.Lerp(ragdollBoneTransforms[i].rotation, standUpBoneTransforms[i].rotation, elapsedPercentage);
        }

        if (elapsedPercentage >= 1)
        {
            DisableRagdoll();

            state = RagdollState.standingUp;
            animator.Play(faceUpStandAnimationStateName);
        }
    }

    private IEnumerator StandingUpBehaviour()
    {
        yield return new WaitForEndOfFrame();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(faceUpStandAnimationStateName))
        {
            state = RagdollState.disabled;

            characterController.enabled = true;
            playerMovement.enabled = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            EnableRagdoll();
            ThrowRagdoll(other);
        }
    }

    private void ThrowRagdoll(Collider other)
    {
        //Vector3 hitDirection = (other.transform.position - transform.position).normalized;
        Vector3 vehicleVelocity = other.attachedRigidbody.velocity;
        Vector3 playerVelocity = characterController.velocity;
        Vector3 relativeVelocity = vehicleVelocity + playerVelocity + Vector3.up * 10;

        Vector3 hitForce = relativeVelocity * collisionForceMulti;


        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            //rb.AddExplosionForce(300f, hitPoint, .1f, 0, ForceMode.Impulse);
            rb.AddForce(hitForce);
        }
        //EnableRagdoll(hitForce);
    }

    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            boneTransforms[i].position = bones[i].localPosition;
            boneTransforms[i].rotation = bones[i].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name != clipName) continue;

            clip.SampleAnimation(gameObject, 0);
            PopulateBoneTransforms(boneTransforms);
            break;
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }
}
