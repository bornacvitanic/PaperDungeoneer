using PaperDungoneer.WordDictionary;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class TypingLogic : MonoBehaviour
    {
        [SerializeField] private WordPicker wordPicker;
        [SerializeField] private int startingDifficultyLevel = 10;
        [SerializeField] private int slidingDifficultyRange = 20;
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
            var scoreWord = wordPicker.GetWordsByScore(difficultyLevel - slidingDifficultyRange, difficultyLevel);

            OnWordPicked.Invoke(scoreWord[Random.Range(0, scoreWord.Count)].word);
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