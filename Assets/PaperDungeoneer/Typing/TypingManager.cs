using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungeoneer.Typing
{
    public class TypingManager : MonoBehaviour
    {
        [SerializeField] private TypingUI typingUI;
        [SerializeField] private TMP_InputField inputField;
        [Header("Colors")]
        [SerializeField] private Color correctColor = Color.green;
        [SerializeField] private Color incorrectColor = Color.red;
        [SerializeField] private Color defaultColor = Color.white;

        private string targetWord;
        private string previousInput = string.Empty;

        public UnityEvent OnWordCompleted;
        public UnityEvent OnCorrectLetterTyped;
        public UnityEvent OnIncorrectLetterTyped;

        public void Start()
        {
            inputField.text = "";
            inputField.onValueChanged.AddListener(ProcessInput);
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

        private void ProcessInput(string currentInput)
        {
            typingUI.SetTargetText("", defaultColor);
            for (int i = 0; i < targetWord.Length; i++)
            {

                if (i < currentInput.Length)
                {
                    var currentLetter = currentInput[i];
                    if (currentLetter == ' ')
                        currentLetter = '\u02fd';

                    if (currentLetter == targetWord[i])
                    {
                        typingUI.AddLetterToTargetText(targetWord[i], correctColor);
                        OnCorrectLetterTyped.Invoke();
                    }
                    else
                    {
                        typingUI.AddLetterToTargetText(currentLetter, incorrectColor);
                        OnIncorrectLetterTyped.Invoke();
                    }
                }
                else
                {
                    typingUI.AddLetterToTargetText(targetWord[i], defaultColor);
                }
            }

            for (int i = targetWord.Length; i < currentInput.Length; i++)
                typingUI.AddLetterToTargetText(currentInput[i], incorrectColor);

            CheckIfWordCompleted(currentInput);
        }

        public void CheckIfWordCompleted(string currentInput)
        {
            if(currentInput == targetWord)
                OnWordCompleted.Invoke();
        }
    }
}