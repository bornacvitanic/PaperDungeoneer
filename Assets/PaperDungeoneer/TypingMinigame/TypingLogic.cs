using PaperDungoneer.WordDictionary;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class TypingLogic : MonoBehaviour
    {
        [SerializeField] private WordPicker wordPicker;
        [SerializeField] private int startingDifficultyLevel = 10;
        [SerializeField] private int increaseInLevel = 2;

        private int difficultyLevel;

        public UnityEvent<string> OnWordPicked;
        public UnityEvent OnRestartGame;
        public UnityEvent OnEndGame;

        private void Start()
        {
            difficultyLevel = startingDifficultyLevel;
            PickTargetWord();
        }

        public void PickTargetWord()
        {
            var word = wordPicker.GetWordsByValue(difficultyLevel);

            OnWordPicked.Invoke(word[Random.Range(0, word.Count)].word);
        }
            
        public void IncreaseDifficulty()
        {
            difficultyLevel += increaseInLevel;
        }

        public void RestartGame()
        {
            difficultyLevel = startingDifficultyLevel;
            OnRestartGame.Invoke();
            PickTargetWord();
        }

        public void EndGame() 
        {
            OnEndGame.Invoke();
        }
    }
}