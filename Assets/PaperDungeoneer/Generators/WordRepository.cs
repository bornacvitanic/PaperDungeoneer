using System.Collections.Generic;
using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    [CreateAssetMenu(menuName = "Words/Word Repository")]
    public class WordRepository : ScriptableObject
    {
        [SerializeField] private List<WordValue> words = new();

        public List<WordValue> Words
        {
            get => words;
            set => words = value;
        }

        private readonly HashSet<char> homeRowKeys = new()
        {
            'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l'
        };

        [ContextMenu("Evaluate Word Values")]
        public void EvaluateWordValues()
        {
            for(int i = 0; i < words.Count; i++)
            {
                int newWordValue = CalculateWordValue(words[i].word);
                words[i] = new WordValue() { word = words[i].word, wordValue = newWordValue };
            }
        }

        private int CalculateWordValue(string word)
        {
            if (string.IsNullOrEmpty(word)) return 0;

            int lengthFactor = word.Length * 2;
            int homeRowFactor = 0;

            foreach (char c in word.ToLower())
            {
                if (!homeRowKeys.Contains(c))
                {
                    homeRowFactor += 2;
                }
            }

            return lengthFactor + homeRowFactor;
        }
    }

}