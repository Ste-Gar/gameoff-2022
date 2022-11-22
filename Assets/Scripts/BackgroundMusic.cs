using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
   // private StudioEventEmitter m_music;
    private RagdollManager playerRagdoll;
    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance money;

    private void Awake()
    {

        playerRagdoll = FindObjectOfType<RagdollManager>();

        playerRagdoll.OnRagdollEnable += EnableComboMusic;
        playerRagdoll.OnRagdollDisable += DisableComboMusic;
    }

    private void OnDestroy()
    {
        playerRagdoll.OnRagdollEnable -= EnableComboMusic;
        playerRagdoll.OnRagdollDisable -= DisableComboMusic;
    }

    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        money = FMODUnity.RuntimeManager.CreateInstance("event:/Money");
        music.start();
    }

    private void EnableComboMusic(object sender, EventArgs e)
    {
        money.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Flight", 1);
        money.start();
        money.setParameterByName("Money End", 0);
    }

    private void DisableComboMusic(object sender, EventArgs e)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Flight", 0);
        money.setParameterByName("Money End", 1);

    }
}
