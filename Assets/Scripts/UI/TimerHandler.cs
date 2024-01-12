using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = GenerateText(GameManager.Instance.GetCurrentTime());
    }

    private string GenerateText(float time)
    {
        int mins = (int)time / 60;
        int secs = (int)time % 60;
        
        // Adds an extra 0 before the number if its single digit
        return "TIME: " + (mins < 10 ? "0" + mins : ""+mins) + ":" + (secs < 10 ? "0" + secs : ""+secs);
    }
}
