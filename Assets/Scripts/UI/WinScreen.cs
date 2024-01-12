using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [Header("Config")] 
    [SerializeField] private Color green;
    [SerializeField] private Color red;

    private void Start()
    {
        int finalScore = GameManager.Instance.FinalScore;
        int highscore = GameManager.Instance.HighScore;
        
        
        scoreText.text = "Score: " + finalScore + " (" + GameManager.Instance.Score + "-" + (int)GameManager.Instance.Timer + "x5)";
        highscoreText.text = "Highscore: " + highscore;
        
        // Make highscore green or red depending on if its new or not
        highscoreText.color = finalScore < highscore ? red : green;
    }

    public void OnRetry()
    {
        GameManager.Instance.RetryGame();
    }
}
