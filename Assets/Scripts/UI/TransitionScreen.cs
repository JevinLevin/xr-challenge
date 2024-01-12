using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private void FadeScreen(float endValue, float length)
    {
        canvasGroup.DOFade(endValue, length);
    }
    
}
