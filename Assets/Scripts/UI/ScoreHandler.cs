using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float tweenInLength;
    [SerializeField] private float tweenOutLength;
    [SerializeField] private float tweenMaxValue;

    
    private TextMeshProUGUI text;

    private float fontSize;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        fontSize = text.fontSize;
    }

    private void OnEnable()
    {
        GameManager.OnUpdateScore += SetScore;
    }
    
    private void OnDisable()
    {
        GameManager.OnUpdateScore -= SetScore;
    }

    /// <summary>
    /// Sets score text based on input
    /// </summary>
    /// <param name="score">The players current score.</param>
    private void SetScore(int score)
    {
        text.text = "SCORE: " + score;

        StartCoroutine(GameManager.Instance.PlayTweenYoYo(value => text.fontSize = value, fontSize, fontSize*tweenMaxValue, tweenInLength, tweenOutLength, Easing.easeOutQuad));

    }
}
