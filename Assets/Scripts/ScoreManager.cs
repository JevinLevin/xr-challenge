using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.OnUpdateScore += SetScore;
    }

    /// <summary>
    /// Sets score text based on input
    /// </summary>
    private void SetScore(int score)
    {
        text.text = "SCORE: " + score;
    }
}
