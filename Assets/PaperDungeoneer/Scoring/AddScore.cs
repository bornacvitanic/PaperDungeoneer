using UnityEngine;

namespace PaperDungeoneer.Scoring
{
    public class AddScore : MonoBehaviour
    {
        private void Start()
        {
            if (ScoreManager.Instance == null)
                Debug.LogWarning("Scorer instance not found! Make sure a Scorer exists in the scene.");
        }

        public void AddPoints(int score)
        {
            ScoreManager.Instance.AddScore(score);
        }
    }
}
