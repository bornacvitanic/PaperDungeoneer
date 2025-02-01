using System.Collections.Generic;
using UnityEngine;

namespace PaperDungoneer.Generators
{
    [CreateAssetMenu(menuName = "Words/Word Dictionary")]
    public class WordDictionary : ScriptableObject
    {
        public string wordsInParagraph = "";
        public List<string> words = new();

        [ContextMenu("Populate From Paragraph")]
        public void PopulateFromParagraph()
        {
            if (string.IsNullOrEmpty(wordsInParagraph))
            {
                Debug.LogWarning("Input text is empty or null.");
                return;
            }

            words.Clear();
            words.AddRange(wordsInParagraph.Split(", "));
        }
    }
}