using UnityEngine;

namespace PaperDungeoneer.Visuals
{
    [RequireComponent(typeof(Light))]
    public class FlickeringLight : MonoBehaviour
    {
        [Header("Flicker Settings")]
        [Tooltip("Minimum intensity of the light.")]
        public float minIntensity = 0.5f;

        [Tooltip("Maximum intensity of the light.")]
        public float maxIntensity = 1.5f;

        [Tooltip("Speed of the flicker effect.")]
        public float flickerSpeed = 1.0f;

        [Tooltip("Smoothness of the flicker effect (higher values = smoother transitions).")]
        public float smoothness = 5.0f;

        [Header("Randomness")]
        [Tooltip("Adds randomness to the flicker effect.")]
        public float randomness = 0.1f;

        private Light _light;
        private float _targetIntensity;
        private float _currentIntensity;

        private void Start()
        {
            _light = GetComponent<Light>();
            _currentIntensity = _light.intensity;
            _targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        private void Update()
        {
            // Smoothly transition to the target intensity
            _currentIntensity = Mathf.Lerp(_currentIntensity, _targetIntensity, Time.deltaTime * smoothness);
            _light.intensity = _currentIntensity;

            // Update the target intensity at random intervals
            if (Mathf.Abs(_currentIntensity - _targetIntensity) < 0.05f)
            {
                _targetIntensity = Random.Range(minIntensity, maxIntensity) + Random.Range(-randomness, randomness);
            }

            // Add subtle variations to simulate flickering
            float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0) * (maxIntensity - minIntensity) + minIntensity;
            _light.intensity = Mathf.Lerp(_light.intensity, flicker, Time.deltaTime * smoothness);
        }
    }
}