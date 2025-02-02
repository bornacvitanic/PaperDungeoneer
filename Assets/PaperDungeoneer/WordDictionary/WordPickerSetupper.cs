using UnityEngine;

namespace PaperDungoneer.WordDictionary
{
    public class WordPickerSetupper : MonoBehaviour
    {
        [SerializeField] private WordRepository wordRepository;

        private void Awake()
        {
            FindAnyObjectByType<WordPicker>().WordRepository = wordRepository;
        }
    }
}