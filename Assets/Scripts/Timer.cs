using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public event EventHandler onTimeOut;

    [SerializeField] Image timerImage;
    [SerializeField] float gameDuration = 180f;
    float elapsedTime;

    private void OnEnable()
    {
        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerVisual();

        if(elapsedTime >= gameDuration)
        {
            onTimeOut?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateTimerVisual()
    {
        timerImage.fillAmount = 1 - (elapsedTime / gameDuration);
    }
}
