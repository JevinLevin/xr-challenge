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

    public void FadeIn(float length, string transitionMsg)
    {
        // Init
        gameObject.SetActive(true);
        canvasGroup.alpha = 0.0f;
        
        screenText.text = transitionMsg;
        FadeScreen(1, length);
    }
    public void FadeOut(float length)
    {
        // Init
        gameObject.SetActive(true);
        canvasGroup.alpha = 1.0f;
        
        FadeScreen(0, length);
    }

    private void FadeScreen(float endValue, float length)
    {
        canvasGroup.DOFade(endValue, length);
    }
    
}
