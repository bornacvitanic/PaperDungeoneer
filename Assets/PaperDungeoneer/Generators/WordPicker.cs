using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    public class WordPicker : MonoBehaviour
    {
        [SerializeField] private WordRepository wordDictionary;

        public List<WordValue> GetWordsByLenght(int length)
        {
            List<WordValue> filteredWords = wordDictionary.Words.Where(word => word.word.Length <= length).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with length {length}");
                return new();
            }
            return filteredWords;
        }

        public List<WordValue> GetWordsByValue(int wordValue)
        {
            List<WordValue> filteredWords = wordDictionary.Words.Where(word => word.wordValue <= wordValue).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with value {wordValue}");
                return new();
            }
            return filteredWords;
        }
    }
}