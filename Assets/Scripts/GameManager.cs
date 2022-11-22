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
    [SerializeField] GameObject gameUI;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        timer.OnTimeOut += EndGame;
    }

    private void OnDestroy()
    {
        timer.OnTimeOut -= EndGame;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameDelay());
    }

    private IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(2);
        timer.enabled = true;
        gameUI.SetActive(true);
        ResetGame();
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
