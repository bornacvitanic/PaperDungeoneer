using UnityEngine;
using System.Collections;

namespace PaperDungoneer.Sounds
{
    public class TimedAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;  // Assign an AudioSource with looping audio
        [SerializeField] private float fadeDuration = 0.05f;  // Duration for fade-out effect

        private float activeTime = 0f; // Tracks how long the sound should play
        private bool isFading = false;
        private bool isPaused = true;  // Track whether the sound is paused

        private void Start()
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource not assigned!");
                return;
            }

            audioSource.loop = true;
            audioSource.volume = 0f;  // Start muted
            audioSource.Play();  // Start playing but immediately pause it
            audioSource.Pause();
        }

        private void Update()
        {
            if (activeTime <= 0) return;
            
            activeTime -= Time.deltaTime;

            if (isPaused)
            {
                ResumeAudio();
            }

            if (activeTime <= 0 && !isFading)
            {
                StartCoroutine(FadeOutAndPause());
            }
            
        }

        public void SetActiveTime(float timeAmount)
        {
            activeTime = timeAmount;
            if (isPaused)
            {
                ResumeAudio();
            }
            else
            {
                audioSource.volume = 1f;  // Instantly ensure it's at full volume
            }
        }

        private void ResumeAudio()
        {
            audioSource.UnPause(); // Resume from the last paused point
            audioSource.volume = 1f;
            isPaused = false;
        }

        private IEnumerator FadeOutAndPause()
        {
            isFading = true;
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Pause(); // Pause instead of stopping
            isPaused = true;
            isFading = false;
        }
    }

}