using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WordGenerator : MonoBehaviour
{
    [SerializeField] List<string> words = new();

    public UnityEvent<string> OnWordPicked;

    private void Start()
    {
        PickRandomWord();
    }

    public void PickRandomWord()
    {
        OnWordPicked.Invoke(words[Random.Range(0, words.Count)]);
    }
}
