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

        if (GUILayout.Button("Populate From Paragraph"))
        {
            PopulateFromParagraph();
            EditorUtility.SetDirty(wordRepository);
        }

        GUILayout.Space(10);

        // Scrollable List of Stored Words
        if (wordRepository.Words != null && wordRepository.Words.Count > 0)
        {
            GUILayout.Label("Stored Words:", EditorStyles.boldLabel);
            wordsScrollPos = EditorGUILayout.BeginScrollView(wordsScrollPos, GUILayout.Height(150));

            foreach (var word in wordRepository.Words)
            {
                GUILayout.Label(word, EditorStyles.wordWrappedLabel);
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

        wordRepository.Words.Clear();
        wordRepository.Words.AddRange(wordsInParagraph.Split(new[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
        Debug.Log($"Word Repository updated with {wordRepository.Words.Count} words.");
    }
}
