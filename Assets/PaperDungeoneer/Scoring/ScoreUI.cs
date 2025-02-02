using TMPro;
using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreManager scorer;
        [SerializeField] private TMP_Text scoreText;

        private void OnEnable()
        {
            scorer.OnScoreChanged += UpdateScoreDisplay;
            UpdateScoreDisplay(scorer.Score);
        }

        private void OnDisable()
        {
            scorer.OnScoreChanged -= UpdateScoreDisplay;
        }

        private void UpdateScoreDisplay(int newScore)
        {
            scoreText.text = $"{newScore}";
        }
    }
}
