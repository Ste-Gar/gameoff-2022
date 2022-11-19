using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public event EventHandler OnTimeOut;

    [SerializeField] Image timerImage;
    [SerializeField] float gameDuration = 180f;
    float elapsedTime;

    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerVisual();

        if(elapsedTime >= gameDuration && !scoreManager.IsComboRunning)
        {
            OnTimeOut?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateTimerVisual()
    {
        timerImage.fillAmount = 1 - (elapsedTime / gameDuration);
    }
}
