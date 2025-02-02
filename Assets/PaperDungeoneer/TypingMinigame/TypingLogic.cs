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
        [SerializeField] private int uppercaseDifficultyLevelStart = 40;

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
            var scoreWords = wordPicker.GetWordsByScore(difficultyLevel - slidingDifficultyRange, difficultyLevel);
            var randomScoreWord = scoreWords[Random.Range(0, scoreWords.Count)];

            if (randomScoreWord.score <= uppercaseDifficultyStart)
                randomScoreWord.word = randomScoreWord.word.ToLower();

            OnWordPicked.Invoke(randomScoreWord.word);
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