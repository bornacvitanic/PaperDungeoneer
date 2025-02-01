using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TypingManager : MonoBehaviour
{
    [SerializeField] private TypingUI typingUI;
    [SerializeField] private TMP_InputField inputField;
    [Header("Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    public UnityEvent OnWordCompleted;

    private string targetWord;
    public void Start()
    {
        inputField.text = "";
        inputField.onValueChanged.AddListener(CheckInput);
    }

    private void Update()
    {
        if (!inputField.isFocused)
            inputField.ActivateInputField();
    }

    public void SetTargetWord(string word)
    {
        targetWord = word;
        typingUI.SetTargetText(targetWord, defaultColor);
        inputField.text = "";
    }

    private void CheckInput(string currentInput)
    {
        typingUI.SetTargetText("", defaultColor);
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                if (currentInput[i] == targetWord[i])
                    typingUI.AddLetterToTargetText(targetWord[i], correctColor);
                else
                    typingUI.AddLetterToTargetText(targetWord[i], incorrectColor);
            }
            else
            {
                typingUI.AddLetterToTargetText(targetWord[i], defaultColor);
            }
        }

        if (IsWordCompleted(currentInput))
            OnWordCompleted.Invoke();
    }

    public bool IsWordCompleted(string currentInput)
    {
        return currentInput == targetWord;
    }
}
