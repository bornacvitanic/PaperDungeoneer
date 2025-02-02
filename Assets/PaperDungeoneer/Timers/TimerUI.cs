using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PaperDungeoneer.Timers
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private CountdownTimer timer;

        private void OnEnable()
        {
            timer.OnTimeTick += UpdateUITimer;
            timer.OnTimeReset += UpdateUITimer;
        }

        private void OnDisable()
        {
            timer.OnTimeTick -= UpdateUITimer;
            timer.OnTimeReset -= UpdateUITimer;
        }

        public void UpdateUITimer()
        {
            var timeSpan = TimeSpan.FromSeconds(timer.CurrentTimeSeconds);
            timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}