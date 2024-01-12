using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.OnUpdateScore += SetScore;
    }

    /// <summary>
    /// Sets score text based on input
    /// </summary>
    private void SetScore(int score)
    {
        text.text = "SCORE: " + score;
    }
}
