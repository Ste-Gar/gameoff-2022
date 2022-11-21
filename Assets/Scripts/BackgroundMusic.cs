using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private StudioEventEmitter m_music;
    private RagdollManager playerRagdoll;

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
        var target = GameObject.Find("BackgroundMusic");
        m_music = target.GetComponent<StudioEventEmitter>();
    }

    private void EnableComboMusic(object sender, EventArgs e)
    {
        m_music.SetParameter("Flight", 1);
    }

    private void DisableComboMusic(object sender, EventArgs e)
    {
        m_music.SetParameter("Flight", 0);
    }
}
