using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PaperDungoneer.Generators
{
    public class WordPicker : MonoBehaviour
    {
        [SerializeField] private WordDictionary wordDictionary;

        public List<string> GetWordsByLenght(int length)
        {
            List<string> filteredWords = wordDictionary.words.Where(word => word.Length <= length).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with length {length}");
                return new();
            }
            return filteredWords;
        }

        public List<string> GetWordsByRegex(string regexPattern, int length = 0)
        {
            Regex regex = new Regex(regexPattern);

            List<string> filteredWords = wordDictionary.words.Where(word => regex.IsMatch(word)).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found matching regex: {regexPattern}");
                return new();
            }
            if (length > 0)
            {
                filteredWords = GetWordsByLenght(length);
            }

            return filteredWords;
        }
    }
}