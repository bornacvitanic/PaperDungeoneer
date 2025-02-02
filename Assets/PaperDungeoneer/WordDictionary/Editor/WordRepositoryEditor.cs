using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PaperDungoneer.WordDictionary;

public class WordRepositoryWindow : EditorWindow
{
    private string wordsInParagraph = "";
    private WordRepository wordRepository;
    private Vector2 wordsScrollPos;
    private Vector2 inputScrollPos;

    [MenuItem("Tools/Word Repository Editor")]
    public static void ShowWindow()
    {
        GetWindow<WordRepositoryWindow>("Word Repository Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Word Repository Editor", EditorStyles.boldLabel);

        wordRepository = (WordRepository)EditorGUILayout.ObjectField("Word Repository", wordRepository, typeof(WordRepository), false);

        if (wordRepository == null)
        {
            EditorGUILayout.HelpBox("Assign a Word Repository asset.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);

        // Scrollable Text Input Field
        GUILayout.Label("Enter Words (comma or space separated):", EditorStyles.label);
        inputScrollPos = EditorGUILayout.BeginScrollView(inputScrollPos, GUILayout.Height(100));
        wordsInParagraph = EditorGUILayout.TextArea(wordsInParagraph, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Populate From Paragraph and Evaluate"))
        {
            PopulateFromParagraph();
            EditorUtility.SetDirty(wordRepository);
        }

        GUILayout.Space(10);

        // Scrollable List of Stored Words
        if (wordRepository.ScoredWords != null && wordRepository.ScoredWords.Count > 0)
        {
            GUILayout.Label("Stored Words:", EditorStyles.boldLabel);
            wordsScrollPos = EditorGUILayout.BeginScrollView(wordsScrollPos, GUILayout.Height(150));

            foreach (var word in wordRepository.ScoredWords)
            {
                GUILayout.Label(word.word, EditorStyles.wordWrappedLabel);
            }

            EditorGUILayout.EndScrollView();
        }
    }

    private void PopulateFromParagraph()
    {
        if (string.IsNullOrEmpty(wordsInParagraph))
        {
            Debug.LogWarning("Input text is empty or null.");
            return;
        }

        wordRepository.ScoredWords.Clear();
        string[] splitWords = wordsInParagraph.Split(new[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var w in splitWords)
        {
            wordRepository.ScoredWords.Add(new ScoredWord() { word = w});
        }
        wordRepository.EvaluateWordValues();
        Debug.Log($"Word Repository updated with {wordRepository.ScoredWords.Count} words and evaluated.");
    }
}