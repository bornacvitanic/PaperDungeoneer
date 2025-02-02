using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    public class WordPicker : MonoBehaviour
    {
        [SerializeField] private WordRepository wordRepository;

        public WordRepository WordRepository { get { return wordRepository; } set { wordRepository = value; } }

        public List<ScoredWord> GetWordsByLenght(int length)
        {
            List<ScoredWord> filteredWords = wordRepository.ScoredWords.Where(word => word.word.Length <= length).ToList();
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found with length {length}");
                return new();
            }
            return filteredWords;
        }

        public List<ScoredWord> GetWordsByScore(int wordScoreTo, int amountOfWords)
        {
            List<ScoredWord> filteredWords = wordRepository.ScoredWords.Where(word => word.score <= wordScoreTo).ToList();

            filteredWords.Sort((x, y) => y.score.CompareTo(x.score));
            
            if (filteredWords.Count == 0)
            {
                Debug.LogWarning($"No words found up to score {wordScoreTo}");
                return new();
            }

            if(filteredWords.Count < amountOfWords)
                amountOfWords = filteredWords.Count;

            return filteredWords.GetRange(0, amountOfWords);
        }
    }
}