using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Runtime.CompilerServices;

public class CrashSound : MonoBehaviour
{
    private RagdollManager playerRagdoll;
    FMOD.Studio.EventInstance crash;
    [SerializeField] float crashInterval = 1f;
    private float crashTimer;

    private void Awake()
    {
        playerRagdoll = FindObjectOfType<RagdollManager>();
        playerRagdoll.OnRagdollThrow += Crash;
        crash = FMODUnity.RuntimeManager.CreateInstance("event:/Crash");
    }
    private void Start()
    {
        crashTimer = crashInterval;
    }
    private void FixedUpdate()
    {
        crashTimer += Time.fixedDeltaTime;

    }

        private void OnDestroy()
    {
        playerRagdoll.OnRagdollThrow -= Crash;
    }

    private void Crash(object sender, Collider vehicle)
    {
        if (crashTimer < crashInterval) return;
        crash.start();
        crashTimer = 0;
    }
}
