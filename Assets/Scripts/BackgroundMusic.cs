using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private Timer timerScript;
    private RagdollManager playerRagdoll;
    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance money;
    FMOD.Studio.EventInstance ambience;
    bool comboMusic;

    private void Awake()
    {
        timerScript = FindObjectOfType<Timer>();
        playerRagdoll = FindObjectOfType<RagdollManager>();

        playerRagdoll.OnRagdollEnable += EnableComboMusic;
        playerRagdoll.OnRagdollDisable += DisableComboMusic;
        timerScript.OnTimeOut += ScoreMusic;
    }

    private void OnDestroy()
    {
        playerRagdoll.OnRagdollEnable -= EnableComboMusic;
        playerRagdoll.OnRagdollDisable -= DisableComboMusic;
        timerScript.OnTimeOut -= ScoreMusic;
    }

    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        money = FMODUnity.RuntimeManager.CreateInstance("event:/Money");
        ambience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience");
        music.start();
        ambience.start();
    }
    private void Update()
    {
        if (timerScript.ElapsedTime > (timerScript.GameDuration * 0.75 ))
            music.setParameterByName("Time", 1);
        if (comboMusic)
            PlaneDistance();
    }
    private void EnableComboMusic(object sender, EventArgs e)
    {
        comboMusic = true;
        if (timerScript.ElapsedTime < timerScript.GameDuration)
        {
        money.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Flight", 1);
        money.start();
        money.setParameterByName("Money End", 0);
        }
    }

    private void DisableComboMusic(object sender, EventArgs e)
    {
        comboMusic=false;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Flight", 0);
        money.setParameterByName("Money End", 1);

    }
    public void StartMusic()
    {
        music.setParameterByName("Title Screen", 1);
    }
    public void ResetMusic()
    {
        ambience.setParameterByName("Distance", 0);
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.setParameterByName("Time", 0);
        music.start();
    }
    private void ScoreMusic(object sender, EventArgs e)
    {
        music.setParameterByName("Time", 2);
        ambience.setParameterByName("Distance", 15);
    }
    private void PlaneDistance  ()
    {
        float distance = Camera.main.transform.position.y;
        ambience.setParameterByName("Distance", distance);
    }
}

