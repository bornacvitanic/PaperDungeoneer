using System.Collections.Generic;
using NUnit.Framework;
using PaperDungoneer.WordDictionary;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.TypingMinigame
{
    public class TypingLogic : MonoBehaviour
    {
        [SerializeField] private WordPicker wordPicker;
        [SerializeField] private int startingDifficultyLevel = 10;
        [UnityEngine.Range(0f,1f)]
        [SerializeField] private float difficultyMultiplier = 0.5f;
        [SerializeField] private int startingSlidingDifficultyRange = 10;
        [SerializeField] private int uppercaseDifficultyLevelStart = 40;

        private int difficultyLevel;
        private int slidingDifficultyRange;

        private List<ScoredWord> fetchedWords = new();

        public UnityEvent<string> OnWordPicked;
        public UnityEvent OnRestartGame;
        public UnityEvent OnEndGame;
        public UnityEvent OnDifficultyLevelIncrease;

        private void Start()
        {
            difficultyLevel = startingDifficultyLevel;
            slidingDifficultyRange = startingSlidingDifficultyRange;
            PickTargetWord();
        }

        public void PickTargetWord()
        {
            if (fetchedWords.Count == 0)
            {
                fetchedWords = wordPicker.GetWordsByScore(difficultyLevel, slidingDifficultyRange, slidingDifficultyRange);
                IncreaseDifficultAndSlidingLevel();
            }

            var randomScoreWord = fetchedWords[Random.Range(0, fetchedWords.Count)];
            fetchedWords.Remove(randomScoreWord);

            if (randomScoreWord.score <= uppercaseDifficultyLevelStart)
                randomScoreWord.word = randomScoreWord.word.ToLower();

            OnWordPicked.Invoke(randomScoreWord.word);
        }

        private void IncreaseDifficultAndSlidingLevel()
        {
            difficultyLevel += (int)(difficultyLevel * difficultyMultiplier);
            slidingDifficultyRange++;

            OnDifficultyLevelIncrease.Invoke();
        }

        public void RestartGame()
        {
            difficultyLevel = startingDifficultyLevel;
            slidingDifficultyRange = startingSlidingDifficultyRange;
            fetchedWords = wordPicker.GetWordsByScore(difficultyLevel, slidingDifficultyRange, slidingDifficultyRange);
            OnRestartGame.Invoke();
            PickTargetWord();
        }

        public void EndGame() 
        {
            OnEndGame.Invoke();
        }
    }
}