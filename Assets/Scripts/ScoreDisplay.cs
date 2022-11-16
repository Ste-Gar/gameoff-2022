using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private enum FadeStatus
    {
        Waiting,
        FadeIn,
        FadeOut
    }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] TextMeshProUGUI totalText;

    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 1f;
    private float fadeTime;
    FadeStatus fadeStatus = FadeStatus.Waiting;

    RagdollManager playerRagdoll;

    private void Awake()
    {
        playerRagdoll = FindObjectOfType<RagdollManager>();
    }

    private void OnEnable()
    {
        playerRagdoll.onRagdollEnable += StartFadeIn;
        playerRagdoll.onRagdollDisable += StartFadeOut;
    }

    private void OnDisable()
    {
        playerRagdoll.onRagdollEnable -= StartFadeIn;
        playerRagdoll.onRagdollDisable -= StartFadeOut;
    }

    private void Start()
    {
        scoreText.alpha = 0;
        multiplierText.alpha = 0;
    }

    private void Update()
    {
        switch (fadeStatus)
        {
            case (FadeStatus.Waiting):
                break;
            case (FadeStatus.FadeIn):
                FadeIn();
                break;
            case (FadeStatus.FadeOut):
                FadeOut();
                break;
        }
    }

    private void StartFadeIn(object sender, EventArgs e)
    {
        fadeStatus = FadeStatus.FadeIn;
    }

    private void StartFadeOut(object sender, EventArgs e)
    {
        fadeStatus = FadeStatus.FadeOut;
    }

    private void FadeIn()
    {
        scoreText.alpha = Mathf.Lerp(0, 1, fadeTime += 1 / fadeInDuration * Time.deltaTime);
        multiplierText.alpha = scoreText.alpha;
        
        if(scoreText.alpha >= 1)
        {
            fadeStatus = FadeStatus.Waiting;
            fadeTime = 0;
        }
    }

    private void FadeOut()
    {
        scoreText.alpha = Mathf.Lerp(1, 0, fadeTime += 1 / fadeOutDuration * Time.deltaTime);
        multiplierText.alpha = scoreText.alpha;

        if (scoreText.alpha <= 0)
        {
            fadeStatus = FadeStatus.Waiting;
            fadeTime = 0;
        }
    }
}
