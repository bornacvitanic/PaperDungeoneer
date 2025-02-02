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

        public List<ScoredWord> GetWordsByValue(int wordValue)
        {
            List<ScoredWord> filteredWords = wordDictionary.ScoredWords.Where(word => word.score <= wordValue).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with value {wordValue}");
                return new();
            }
            return filteredWords;
        }
    }
}