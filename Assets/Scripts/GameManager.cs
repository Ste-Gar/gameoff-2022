using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        End
    }

    public static GameState gameState;

    private Timer timer;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        timer.onTimeOut += EndGame;
    }

    private void OnDestroy()
    {
        timer.onTimeOut -= EndGame;
    }

    private void EndGame(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
