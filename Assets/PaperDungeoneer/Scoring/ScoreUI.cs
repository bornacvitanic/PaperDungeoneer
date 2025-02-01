using TMPro;
using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private ScoreManager scorer;

        private void OnEnable()
        {
            if (scorer != null)
            {
                scorer.OnScoreChanged += UpdateScoreDisplay;
            }
        }

        private void OnDisable()
        {
            if (scorer != null)
            {
                scorer.OnScoreChanged -= UpdateScoreDisplay;
            }
        }

        private void UpdateScoreDisplay(int newScore)
        {
            scoreText.text = $"{newScore}";
        }
    }
}
