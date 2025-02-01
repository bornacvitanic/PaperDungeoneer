using TMPro;
using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private void OnEnable()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
                UpdateScoreDisplay(ScoreManager.Instance.Score); // Initialize UI with current score
            }
        }

        private void OnDisable()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
            }
        }

        private void UpdateScoreDisplay(int newScore)
        {
            scoreText.text = $"{newScore}";
        }
    }
}
