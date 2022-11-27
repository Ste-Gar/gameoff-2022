using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] TextMeshProUGUI finalScoreText;

    float currentCombo;
    float currentMultiplier;
    float totalScore;

    private RagdollManager playerRagdoll;

    private bool isComboRunning;
    public bool IsComboRunning { get { return isComboRunning; } }
    readonly string format = "###,###,###,###.##";

    private void Awake()
    {
        playerRagdoll = FindObjectOfType<RagdollManager>();
    }

    private void OnEnable()
    {
        playerRagdoll.OnRagdollEnable += StartCombo;
        playerRagdoll.OnRagdollDisable += EndCombo;
        playerRagdoll.OnRagdollThrow += UpdateMultiplier;
        GameManager.OnGameReset += ResetScore;
    }

    private void OnDisable()
    {
        playerRagdoll.OnRagdollEnable -= StartCombo;
        playerRagdoll.OnRagdollDisable -= EndCombo;
        playerRagdoll.OnRagdollThrow -= UpdateMultiplier;
        GameManager.OnGameReset -= ResetScore;
    }

    private void Update()
    {
        if (isComboRunning)
        {
            UpdateCombo();
        }
    }

    private void UpdateCombo()
    {
        currentCombo += Time.deltaTime * 100;
        scoreText.text = $"${currentCombo.ToString(format)}";
    }

    private void UpdateMultiplier(object sender, Collider vehicle)
    {
        currentMultiplier += vehicle.gameObject.GetComponentInParent<VehicleScore>().ScoreMultiplier;
        multiplierText.text = $"x{currentMultiplier}";
    }

    private void StartCombo(object sender, EventArgs e)
    {
        isComboRunning = true;
    }

    private void EndCombo(object sender, EventArgs e)
    {
        UpdateTotal();
        isComboRunning = false;
        currentCombo = 0;
        currentMultiplier = 0;
    }

    private void UpdateTotal()
    {
        if (GameManager.gameState != GameManager.GameState.Playing) return;

        totalScore += currentCombo * currentMultiplier;
        totalText.text = $"${totalScore.ToString(format)}";
    }

    private void ResetScore(object sender, EventArgs e)
    {
        EndCombo(sender, e);
        totalScore = 0;
        totalText.text = "$ 0";
    }

    internal void UpdateFinalScore()
    {
        if (GameManager.gameState != GameManager.GameState.Playing) return;

        finalScoreText.text = $"${totalScore.ToString(format)}";
    }
}
