using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text targetWordDisplay;
    private string targetWord;

    public UnityEvent OnWordCompleted;

    void Start()
    {
        inputField.onValueChanged.AddListener(UpdateColoredText);
    }

    private void Update()
    {
        if (!inputField.isFocused)
            inputField.ActivateInputField();
    }

    public void AssignTargetWord(string word)
    {
        targetWord = word;
        targetWordDisplay.text = $"<color=#{ColorUtility.ToHtmlStringRGB(defaultColor)}>{targetWord}</color>";
        inputField.text = "";
        inputField.ActivateInputField(); 
    }

    void UpdateColoredText(string currentInput)
    {
        string coloredText = "";
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                if (currentInput[i] == targetWord[i])
                    coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(correctColor)}>{targetWord[i]}</color>";
                else
                    coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(incorrectColor)}>{targetWord[i]}</color>";
            }
            else
            {
                coloredText += targetWord[i];
            }
        }
        targetWordDisplay.text = coloredText;
        CheckInput(currentInput);
    }

    private void CheckInput(string currentInput)
    {
        if (currentInput != targetWord) return;

        inputField.text = "";
        targetWordDisplay.text = "";
        Debug.Log("Correct! Word defeated!");
        OnWordCompleted.Invoke();
    }
}
