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

    private float health;
    private float maxHealth;

    /// <summary>
    /// Inititalises the healthbar
    /// </summary>
    public void Setup(float maxHealth, ref Action<float> onHealthChange)
    {
        this.maxHealth = maxHealth;
        onHealthChange += OnHealthChange;
    }

    /// <summary>
    /// Called by the parent whenever their health is changed
    /// By assigning it to an event it avoids needing to be checked every frame in update
    /// </summary>
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
    }

}
