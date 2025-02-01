using PaperDungoneer.Generators;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class TypingLogic : MonoBehaviour
    {
        [SerializeField] private WordPicker wordPicker;

        public UnityEvent<string> OnWordPicked;

        private void Start()
        {
            PickTargetWord();
        }

        public void PickTargetWord()
        {
            var word = wordPicker.GetWordsByLenght(10);

            OnWordPicked.Invoke(word[Random.Range(0, word.Count)]);
        }
    }
}