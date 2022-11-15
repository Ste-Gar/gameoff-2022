using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalText;

    float currentCombo;
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
    }

    private void OnDisable()
    {
        playerRagdoll.onRagdollEnable -= StartCombo;
        playerRagdoll.onRagdollDisable -= EndCombo;
    }

    private void Update()
    {
        if (isScoring)
        {
            currentCombo += Time.deltaTime * 100;
            UpdateCombo();
        }
    }

    private void UpdateCombo()
    {
        scoreText.text = $"${currentCombo.ToString(format)}";
    }

    private void UpdateTotal()
    {
        totalText.text = $"${totalScore.ToString(format)}";
    }

    private void StartCombo(object sender, EventArgs e)
    {
        isScoring = true;
    }

    private void EndCombo(object sender, EventArgs e)
    {
        totalScore += currentCombo;
        UpdateTotal();
        isScoring = false;
        currentCombo = 0;
    }
}
