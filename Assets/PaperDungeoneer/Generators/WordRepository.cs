using System.Collections.Generic;
using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    [CreateAssetMenu(menuName = "Words/Word Dictionary")]
    public class WordRepository : ScriptableObject
    {
        [SerializeField] private List<string> words = new();

        public List<string> Words { get { return words; } set { words = value; } }
    }
}