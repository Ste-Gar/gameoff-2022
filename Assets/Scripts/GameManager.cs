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

    private bool isPaused;

    PlayerMovement playerMovement;

    private Timer timer;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseUI;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Playing)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
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

    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
