using UnityEngine;
using UnityEngine.Events;

namespace PaperDungeoneer.Utility
{
    public class CanvasFader : MonoBehaviour
    {
        [Header("References")]
        public CanvasGroup canvasGroup; // Assign the CanvasGroup component of your black canvas.

        [Header("Settings")]
        public float fadeOutDuration = 1f; // Duration for fade-out.
        public AnimationCurve fadeOutAnimationCurve = AnimationCurve.Linear(0,0,1,1);
        public float fadeInDuration = 1f; // Duration for fade-in.
        public AnimationCurve fadeInAnimationCurve = AnimationCurve.Linear(0,1,1,0);

        [Header("Events")]
        public UnityEvent onFadeOutComplete; // Raised when fade-out is done.
        public UnityEvent onFadeInStart; // Raised when fade-in starts.
        public UnityEvent onCrossFadeMidpoint; // Raised at the midpoint of a crossfade.

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
            currentFadeCoroutine = null;
        }

        private System.Collections.IEnumerator FadeOutCoroutine()
        {
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
            onCrossFadeMidpoint.Invoke();

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
            currentFadeCoroutine = null;
        }
    }
}