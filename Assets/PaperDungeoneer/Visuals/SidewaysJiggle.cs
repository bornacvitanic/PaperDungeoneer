using UnityEngine;

namespace PaperDungeoneer.Visuals
{
    public class SidewaysJiggle : MonoBehaviour
    {
        [SerializeField] private float jiggleAngle = 15f; // Angle for the jiggle (+- from 0)
        [SerializeField] private float jiggleInterval = 0.5f; // Time interval between jiggles

        private GameObject target; // The target GameObject to jiggle
        private float timer;
        private bool isJiggling;

        private void Start()
        {
            // Start jiggling automatically
            StartJiggle();
        }

        private void Update()
        {
            if (target == null || !isJiggling) return;

            // Update the timer
            timer += Time.deltaTime;

            // Check if it's time to jiggle
            if (timer >= jiggleInterval)
            {
                Jiggle();
                timer = 0f;
            }
        }

        // Public method to set the target GameObject
        public void SetTarget(GameObject newTarget)
        {
            if (newTarget != null)
            {
                target = newTarget;
            }
        }

        // Public method to start the jiggle
        public void StartJiggle()
        {
            isJiggling = true;
        }

        // Public method to stop the jiggle
        public void StopJiggle()
        {
            isJiggling = false;
        }

        // Perform the jiggle by jumping between +angle and -angle
        private void Jiggle()
        {
            if (target == null) return;

            // Alternate between +angle and -angle
            float currentAngle = target.transform.rotation.eulerAngles.z;
            float newAngle = Mathf.Approximately(currentAngle, jiggleAngle) ? -jiggleAngle : jiggleAngle;

            // Apply the new rotation
            target.transform.rotation = Quaternion.Euler(target.transform.eulerAngles.x, target.transform.eulerAngles.y, newAngle);
        }
    }
}