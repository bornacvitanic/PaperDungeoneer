using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace PaperDungeoneer.Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        private int score;

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
            Score = 0;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}