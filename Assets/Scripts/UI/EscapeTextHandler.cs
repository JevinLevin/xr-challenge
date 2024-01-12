using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EscapeTextHandler : MonoBehaviour
{
    private TextMeshProUGUI escapeText;
    
    private void OnEnable()
    {
        GameManager.OnEscapeStart += ActivateText;
    }

    private void OnDisable()
    {
        GameManager.OnEscapeStart -= ActivateText;
    }

    private void Awake()
    {
        escapeText = GetComponent<TextMeshProUGUI>();

        escapeText.alpha = 0.0f;
    }

    private void ActivateText()
    {
        escapeText.alpha = 1.0f;
    }
}
