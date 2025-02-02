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
        private string currentWord = "";

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
            var scoreWords = wordPicker.GetWordsByScore(difficultyLevel, slidingDifficultyRange);
            var randomScoreWord = scoreWords[Random.Range(0, scoreWords.Count)];

            while(currentWord == randomScoreWord.word)
            {
                randomScoreWord = scoreWords[Random.Range(0, scoreWords.Count)];
            }

            currentWord = randomScoreWord.word;
            if (randomScoreWord.score <= uppercaseDifficultyLevelStart)
                currentWord = randomScoreWord.word.ToLower();

            OnWordPicked.Invoke(currentWord);
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