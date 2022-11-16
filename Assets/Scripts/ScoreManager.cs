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

    float currentCombo;
    float currentMultiplier;
    float totalScore;

    private RagdollManager playerRagdoll;

    private bool isScoring;
    readonly string format = "###,###,###,###.##";

    private void Awake()
    {
        playerRagdoll = FindObjectOfType<RagdollManager>();
    }

    private void OnEnable()
    {
        playerRagdoll.onRagdollEnable += StartCombo;
        playerRagdoll.onRagdollDisable += EndCombo;
        playerRagdoll.onRagdollThrow += UpdateMultiplier;
    }

    private void OnDisable()
    {
        playerRagdoll.onRagdollEnable -= StartCombo;
        playerRagdoll.onRagdollDisable -= EndCombo;
        playerRagdoll.onRagdollThrow -= UpdateMultiplier;
    }

    private void Update()
    {
        if (isScoring)
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
        currentMultiplier += vehicle.gameObject.GetComponent<Vehicle>().ScoreMultiplier;
        multiplierText.text = $"x{currentMultiplier}";
    }

    private void StartCombo(object sender, EventArgs e)
    {
        isScoring = true;
    }

    private void EndCombo(object sender, EventArgs e)
    {
        UpdateTotal();
        isScoring = false;
        currentCombo = 0;
        currentMultiplier = 0;
    }

    private void UpdateTotal()
    {
        totalScore += currentCombo * currentMultiplier;
        totalText.text = $"${totalScore.ToString(format)}";
    }
}
