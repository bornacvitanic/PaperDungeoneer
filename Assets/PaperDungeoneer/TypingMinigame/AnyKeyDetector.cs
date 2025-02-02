using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class AnyKeyDetector : MonoBehaviour
    {
        public UnityEvent OnAnyKeyPressed;

        private void Update()
        {
            if (Input.anyKey)
            {
                OnAnyKeyPressed.Invoke();
                this.enabled = false;
            }
        }
    }
}
