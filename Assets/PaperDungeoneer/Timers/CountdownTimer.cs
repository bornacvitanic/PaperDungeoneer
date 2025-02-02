using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungeoneer.Timers
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private float timeSeconds;

        private float currentTimeSeconds;
        private Coroutine timerCoroutine;

        public float CurrentTimeSeconds => currentTimeSeconds;

        public Action OnTimeTick;
        public Action OnTimeReset;

        public UnityEvent OnTimerComplete;

        private void Awake()
        {
            ResetTime();
        }

        private void OnDisable()
        {
            StopTimer();
        }

        [ContextMenu("Start Timer")]
        public void StartTimer()
        {
            StopTimer();
            timerCoroutine = StartCoroutine(CountDown());
        }

        [ContextMenu("Stop Timer")]
        public void StopTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }

        [ContextMenu("Reset Timer")]
        public void ResetTimer()
        {
            currentTimeSeconds = timeSeconds;
            StartTimer();
        }

        public void ResetTime()
        {
            currentTimeSeconds = timeSeconds;
            OnTimeReset();
        }

        private IEnumerator CountDown()
        {
            while (currentTimeSeconds > 0)
            {
                OnTimeTick.Invoke();
                yield return new WaitForSeconds(Time.deltaTime);
                currentTimeSeconds -= Time.deltaTime;
            }

            currentTimeSeconds = 0; 
            OnTimeTick.Invoke();
            OnTimerComplete?.Invoke();
        }
    }
}