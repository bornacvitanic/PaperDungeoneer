using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class AnyKeyDetector : MonoBehaviour
    {
        [SerializeField] private bool detectOnlyOnce;

        public UnityEvent OnAnyKeyPressed;

        private void Update()
        {
            if (Input.anyKey)
            {
                OnAnyKeyPressed.Invoke();
                if(detectOnlyOnce)
                    this.enabled = false;
            }
        }
    }
}
