using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [System.Serializable]
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

    public event EventHandler onRagdollEnable;
    public event EventHandler onRagdollDisable;
    public event EventHandler<Collider> onRagdollThrow;

    RagdollState state = RagdollState.disabled;
    private Rigidbody[] ragdollRigidbodies;
    public Rigidbody[] RagdollRigidbodies { get { return ragdollRigidbodies; } }
    private Transform hipsBone;

    private BoneTransform[] standFaceUpBoneTransforms;
    private BoneTransform[] standFaceDownBoneTransforms;
    private BoneTransform[] ragdollBoneTransforms;
    private Transform[] bones;
    private float bonesResetElapsedTime;
    private bool isFacingUp;

    RagdollMovement ragdollMovement;
    CapsuleCollider playerCollider;
    CharacterController characterController;
    PlayerMovement playerMovement;
    [SerializeField] Animator animator;

    [SerializeField] string faceUpStandAnimationStateName;
    [SerializeField] string faceUpStandAnimationClipName;
    [SerializeField] string faceDownStandAnimationStateName;
    [SerializeField] string faceDownStandAnimationClipName;
    [SerializeField] float timeToResetBones = 0.5f;

    [SerializeField] float collisionForceMulti = 100;
    [SerializeField] float verticalForceMulti = 10;
    private GameObject lastVehicleHit;

    float lastCollisionTime;
    [SerializeField] float ragdollCollisionInterval = 0.3f;

    [SerializeField] float standUpVelocityThreshold = 0.3f;
    [SerializeField] float standUpDelay = 1.0f;
    float standUpTimer;

    private void Awake()
    {
        RagdollCollision.OnAnyRagdollVehicleCollision += OnRagdollVehicleCollision;

        ragdollMovement = GetComponent<RagdollMovement>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider>();

        hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);

        bones = hipsBone.GetComponentsInChildren<Transform>();
        standFaceUpBoneTransforms = new BoneTransform[bones.Length];
        standFaceDownBoneTransforms = new BoneTransform[bones.Length];
        ragdollBoneTransforms = new BoneTransform[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            standFaceUpBoneTransforms[i] = new BoneTransform();
            standFaceDownBoneTransforms[i] = new BoneTransform();
            ragdollBoneTransforms[i] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(faceUpStandAnimationClipName, standFaceUpBoneTransforms);
        PopulateAnimationStartBoneTransforms(faceDownStandAnimationClipName, standFaceDownBoneTransforms);
    }

    private void OnDestroy()
    {
        RagdollCollision.OnAnyRagdollVehicleCollision -= OnRagdollVehicleCollision;
    }

    private void OnRagdollVehicleCollision(object sender, Collision other)
    {
        //if (Time.time - lastCollisionTime < ragdollCollisionInterval) return;

        //lastCollisionTime = Time.time;
        ThrowRagdoll(other.collider);
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
            case RagdollState.resettingBones:
                ResettingBonesBehaviour();
                break;
            case RagdollState.standingUp:
                StandingUpBehaviour();
                break;
        }
    }

    private void RagdollBehaviour()
    {
        if (ragdollRigidbodies[0].velocity.sqrMagnitude < standUpVelocityThreshold)
        {
            standUpTimer += Time.deltaTime;
        }
        else
        {
            standUpTimer = 0;
        }

        if (standUpTimer >= standUpDelay)
        {
            standUpTimer = 0;
            isFacingUp = SetFacingUp();

            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(ragdollBoneTransforms);
            state = RagdollState.resettingBones;
            bonesResetElapsedTime = 0;
        }
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        //playerCollider.enabled = true;
        ragdollMovement.enabled = false;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        onRagdollDisable?.Invoke(this, EventArgs.Empty);
    }

    private void EnableRagdoll()
    {
        characterController.enabled = false;
        playerMovement.enabled = false;
        animator.enabled = false;
        playerCollider.enabled = false;
        ragdollMovement.enabled = true;

        state = RagdollState.enabled;
        onRagdollEnable?.Invoke(this, EventArgs.Empty);
    }

    private void AlignRotationToHips()
    {
        Vector3 currentHipsPosition = hipsBone.position;
        Quaternion currentHipsRotation = hipsBone.rotation;

        Vector3 desiredDirection = isFacingUp ? hipsBone.up * -1 : hipsBone.up;
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

        Vector3 positionOffset = standFaceUpBoneTransforms[0].position;
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

        playerCollider.enabled = true;

        if (isFacingUp)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localPosition = Vector3.Lerp(ragdollBoneTransforms[i].position, standFaceUpBoneTransforms[i].position, elapsedPercentage);
                bones[i].localRotation = Quaternion.Lerp(ragdollBoneTransforms[i].rotation, standFaceUpBoneTransforms[i].rotation, elapsedPercentage);
            }
        }
        else
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localPosition = Vector3.Lerp(ragdollBoneTransforms[i].position, standFaceDownBoneTransforms[i].position, elapsedPercentage);
                bones[i].localRotation = Quaternion.Lerp(ragdollBoneTransforms[i].rotation, standFaceDownBoneTransforms[i].rotation, elapsedPercentage);
            }
        }

        if (elapsedPercentage >= 1)
        {
            state = RagdollState.standingUp;
            DisableRagdoll();

            if (isFacingUp)
                animator.Play(faceUpStandAnimationStateName);
            else
                animator.Play(faceDownStandAnimationStateName);
        }
    }

    private bool SetFacingUp()
    {
        if(Vector3.Dot(hipsBone.transform.TransformDirection(Vector3.forward), Vector3.up) < 0)
        {
            return false;
        }
        return true;
    }


    private void StandingUpBehaviour()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(faceUpStandAnimationStateName) && !animator.GetCurrentAnimatorStateInfo(0).IsName(faceUpStandAnimationStateName))
        {
            state = RagdollState.disabled;

            characterController.enabled = true;
            playerMovement.enabled = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastCollisionTime < 1)
        {
            lastVehicleHit = null;      //reset last vehicle hit after one second
        }
        if (other.CompareTag("Vehicle") && other.gameObject != lastVehicleHit)
        {
            lastVehicleHit = other.gameObject;
            EnableRagdoll();
            ThrowRagdoll(other);
        }
    }

    private void ThrowRagdoll(Collider other)
    {
        if (Time.time - lastCollisionTime < ragdollCollisionInterval) return;
        onRagdollThrow?.Invoke(this, other);

        lastCollisionTime = Time.time;

        //Vector3 hitDirection = (other.transform.position - transform.position).normalized;
        Vector3 vehicleVelocity = other.attachedRigidbody.velocity;
        Vector3 playerVelocity = characterController.velocity;
        Vector3 relativeVelocity = -vehicleVelocity + playerVelocity + Vector3.up * verticalForceMulti;

        Vector3 hitForce = relativeVelocity * collisionForceMulti;


        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.AddForce(hitForce);
        }
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
