using UnityEngine;
using UnityEngine.Events;

namespace PaperDungeoneer.Utility
{
    public class CanvasFader : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup; // Assign the CanvasGroup component of your black canvas.

        [Header("Settings")]
        [SerializeField] private float fadeOutDuration = 1f; // Duration for fade-out.
        [SerializeField] private AnimationCurve fadeOutAnimationCurve = AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float fadeInDuration = 1f; // Duration for fade-in.
        [SerializeField] private AnimationCurve fadeInAnimationCurve = AnimationCurve.Linear(0,1,1,0);

        [Header("Events")]
        public UnityEvent onFadeOutStart; // Raised when fade-out starts.
        public UnityEvent onFadeOutComplete; // Raised when fade-out is done.
        public UnityEvent onCrossFadeMidpoint; // Raised at the midpoint of a crossfade.
        public UnityEvent onFadeInStart; // Raised when fade-in starts.
        public UnityEvent onFadeInComplete; // Raised when fade-in is done.

        private Coroutine currentFadeCoroutine;

        private void Awake()
        {
            if (canvasGroup != null) return;
            Debug.LogError("CanvasGroup is not assigned!");
            enabled = false;
        }

        // Fade from black to transparent
        [ContextMenu("Fade In")]
        public void FadeIn()
        {
            if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = StartCoroutine(FadeInCoroutine());
        }

        // Fade from transparent to black
        [ContextMenu("Fade Out")]
        public void FadeOut()
        {
            if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = StartCoroutine(FadeOutCoroutine());
        }

        // CrossFade: Fade to black and then back to transparent
        [ContextMenu("CrossFade")]
        public void CrossFade()
        {
            if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = StartCoroutine(CrossFadeCoroutine());
        }

        private System.Collections.IEnumerator FadeInCoroutine()
        {
            onFadeInStart.Invoke();
            
            float targetAlpha = 0f;
            float timer = 0f;

            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeInDuration);
                canvasGroup.alpha = fadeInAnimationCurve.Evaluate(t);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onFadeInComplete.Invoke();
            currentFadeCoroutine = null;
        }

        private System.Collections.IEnumerator FadeOutCoroutine()
        {
            onFadeOutStart.Invoke();
            float targetAlpha = 1f;
            float timer = 0f;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeOutDuration);
                canvasGroup.alpha = fadeOutAnimationCurve.Evaluate(t);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onFadeOutComplete.Invoke();
            currentFadeCoroutine = null;
        }

        private System.Collections.IEnumerator CrossFadeCoroutine()
        {
            // Fade to black
            onFadeOutStart.Invoke();
            float targetAlpha = 1f;
            float timer = 0f;

            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeInDuration);
                canvasGroup.alpha = fadeOutAnimationCurve.Evaluate(t);;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onFadeOutComplete.Invoke();
            onCrossFadeMidpoint.Invoke();
            onFadeInStart.Invoke();
            // Fade back to transparent
            targetAlpha = 0f;
            timer = 0f;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeOutDuration);
                canvasGroup.alpha = fadeInAnimationCurve.Evaluate(t);;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onFadeInComplete.Invoke();
            currentFadeCoroutine = null;
        }
    }
}