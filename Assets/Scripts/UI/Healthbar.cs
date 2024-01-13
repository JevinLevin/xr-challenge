using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image image;

    [Header("Config")] 
    [SerializeField] private Color colorLow;
    [SerializeField] private Color colorHigh;
    [SerializeField] private AnimationCurve colorCurve;

    [SerializeField] private float tweenInLength;
    [SerializeField] private float tweenOutLength;
    [SerializeField] private float tweenMaxValue;
    [SerializeField] private bool playTween;

    private float health;
    private float maxHealth;

    private Vector3 startingScale;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        startingScale = transform.localScale;
    }

    private void Update()
    {
        transform.forward = mainCamera.transform.forward;
    }

    /// <summary>
    /// Inititalises the healthbar
    /// </summary>
    /// <param name="maxHealth">The max value of the healthbar.</param>
    /// <param name="onHealthChange">Action to be run every time the parent object takes damage.</param>
    public void Setup(float maxHealth, ref Action<float> onHealthChange)
    {
        this.maxHealth = maxHealth;
        onHealthChange += OnHealthChange;
    }

    /// <summary>
    /// Called by the parent whenever their health is changed
    /// By assigning it to an event it avoids needing to be checked every frame in update
    /// </summary>
    /// <param name="health">Value to change the healthbars value to.</param>
    private void OnHealthChange(float health)
    {
        this.health = health;
        AdjustBarProgress();
    }

    /// <summary>
    /// Changes the healthbar length and color depending on health remaining
    /// </summary>
    private void AdjustBarProgress()
    {
        float t = health / maxHealth;
        image.fillAmount = t;
        image.color = Color.Lerp(colorLow, colorHigh, colorCurve.Evaluate(t));
        
        if(t > 0 && playTween)
            StartCoroutine(GameManager.Instance.PlayTweenYoYo(value => transform.localScale = new Vector3(value, value, value), startingScale.x, startingScale.x*tweenMaxValue, tweenInLength, tweenOutLength, Easing.easeOutQuad));
    }

}
