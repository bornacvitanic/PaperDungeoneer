using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class AddScore : MonoBehaviour
    {
        private ScoreManager scorer;

        private void Start()
        {
            scorer = ScoreManager.Instance;
            if (scorer == null)
            {
                Debug.LogWarning("Scorer instance not found! Make sure a Scorer exists in the scene.");
            }
        }

        public void AddPoints(int score)
        {
            if (scorer != null)
            {
                scorer.AddScore(score);
            }
            else
            {
                Debug.LogWarning("Scorer instance is null. Score was not added.");
            }
        }
    }
}
