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
        }

        private void OnDisable()
        {
            timer.OnTimeTick -= UpdateUITimer;
        }

        public void UpdateUITimer(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}