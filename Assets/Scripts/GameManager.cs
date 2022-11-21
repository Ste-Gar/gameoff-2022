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

    public static event EventHandler OnGameReset;

    public static GameState gameState;

    private Timer timer;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        timer.OnTimeOut += EndGame;
    }

    private void OnDestroy()
    {
        timer.OnTimeOut -= EndGame;
    }

    private void EndGame(object sender, EventArgs e)
    {
        if (gameState == GameState.Playing)
            ResetGame();
    }

    public void ResetGame()
    {
        gameState = GameState.Playing;
        OnGameReset?.Invoke(this, EventArgs.Empty);
    }

    //private void EndGame(object sender, EventArgs e)
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //        OnGameReset.Invoke(this, EventArgs.Empty);
    //}
}
