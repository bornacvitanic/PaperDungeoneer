using TMPro;
using UnityEngine;

namespace PaperDungeoneer.Typing
{
    public class TypingUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text targetWordDisplay;

        public void ResetTargetText()
        {
            targetWordDisplay.text = "";
        }

        public void SetTargetText(string targetedText, Color textColor)
        {
            targetWordDisplay.text = $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{targetedText}</color>";
        }

        public void AddLetterToTargetText(char letter, Color letterColor)
        {
            targetWordDisplay.text += $"<color=#{ColorUtility.ToHtmlStringRGB(letterColor)}>{letter}</color>";
        }
    }
}