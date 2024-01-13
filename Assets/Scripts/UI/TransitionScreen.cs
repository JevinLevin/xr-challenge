using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI screenText;


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Increases opacity of screen
    /// Additionally sets screen text if provided
    /// </summary>
    /// <param name="length">Duration in seconds of the fade</param>
    /// <param name="transitionMsg">Text to display on the screen during the transition.</param>
    public void FadeIn(float length, string transitionMsg = "")
    {
        // Init
        gameObject.SetActive(true);
        canvasGroup.alpha = 0.0f;
        
        screenText.text = transitionMsg;
        FadeScreen(1, length);
    }
    
    /// <summary>
    /// Decreases opacity of screen
    /// </summary>
    /// <param name="length">Duration in seconds of the fade</param>
    public void FadeOut(float length)
    {
        // Init
        gameObject.SetActive(true);
        canvasGroup.alpha = 1.0f;
        
        FadeScreen(0, length);
    }

    /// <summary>
    /// Tweens screen opacity based on parameters
    /// </summary>
    /// <param name="endValue">The final value to tween the opacity of the transition screen</param>
    /// <param name="length">Duration in seconds of the fade</param>
    private void FadeScreen(float endValue, float length)
    {
        StartCoroutine(GameManager.Instance.PlayTween(value => canvasGroup.alpha = value, canvasGroup.alpha, endValue, length, Easing.easeOutQuad));
    }


}
