using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    public void UpdateUITimer(float seconds)
    {
        var timeSpan = TimeSpan.FromSeconds(seconds);
        timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
}
