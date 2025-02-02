using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    public class WordPicker : MonoBehaviour
    {
        [SerializeField] private WordRepository wordDictionary;

        public List<ScoredWord> GetWordsByLenght(int length)
        {
            List<ScoredWord> filteredWords = wordDictionary.ScoredWords.Where(word => word.word.Length <= length).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with length {length}");
                return new();
            }
            return filteredWords;
        }

        public List<ScoredWord> GetWordsByScore(int wordScoreTo, int range)
        {
            if(wordScoreTo > wordDictionary.MaxScore)
                wordScoreTo = wordDictionary.MaxScore;

            List<ScoredWord> filteredWords = wordDictionary.ScoredWords.Where(word => word.score >= wordScoreTo - range && word.score <= wordScoreTo).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found within scores {wordScoreTo - range} - {wordScoreTo}");
                return new();
            }
            return filteredWords;
        }
    }
}