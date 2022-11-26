using Cinemachine;
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

    private float cameraBlendDuration;

    PlayerMovement playerMovement;

    private Timer timer;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject gameOverUI;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraBlendDuration = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
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
        yield return new WaitForSeconds(cameraBlendDuration);
        timer.enabled = true;
        gameUI.SetActive(true);
        ResetGame();
    }

    public void EndGame(object sender, EventArgs e)
    {
        gameState = GameState.End;
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerMovement.enabled = false;
    }

    public void ResetGame()
    {
        gameState = GameState.Playing;
        OnGameReset?.Invoke(this, EventArgs.Empty);
    }
}
