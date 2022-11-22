using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class ThirdPersonCameraController : MonoBehaviour
{
    private float yMaxSpeed;
    private float xMaxSpeed;

    private Timer timer;

    private CinemachineFreeLook cmFreeLook;

    private void Awake()
    {
        cmFreeLook = GetComponent<CinemachineFreeLook>();
        yMaxSpeed = cmFreeLook.m_YAxis.m_MaxSpeed;
        xMaxSpeed = cmFreeLook.m_XAxis.m_MaxSpeed;

        GameManager.OnGameReset += ResetCamera;
        timer = FindObjectOfType<Timer>();
        timer.OnTimeOut += StopCamera;
    }

    private void OnDestroy()
    {
        GameManager.OnGameReset -= ResetCamera;
        timer.OnTimeOut -= StopCamera;
    }

    private void Start()
    {
        StopCamera(this, EventArgs.Empty);
    }

    private void ResetCamera(object sender, EventArgs e)
    {
        cmFreeLook.m_YAxis.m_MaxSpeed = yMaxSpeed;
        cmFreeLook.m_XAxis.m_MaxSpeed = xMaxSpeed;
    }

    private void StopCamera(object sender, EventArgs e)
    {
        cmFreeLook.m_YAxis.m_MaxSpeed = 0;
        cmFreeLook.m_XAxis.m_MaxSpeed = 0;
    }
}
