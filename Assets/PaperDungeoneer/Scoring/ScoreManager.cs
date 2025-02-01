using System;
using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        private int score = 0;

        public event Action<int> OnScoreChanged;

        public int Score
        {
            get => score;
            private set
            {
                score = value;
                OnScoreChanged?.Invoke(score);
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }

        public void SubtractScore(int amount)
        {
            Score -= amount;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}