using UnityEngine;
using UnityEngine.Rendering;

namespace PaperDungeoneer.Utility
{
    public class DisableDebugManager : MonoBehaviour
    {
        private void Awake()
        {
            DebugManager.instance.enableRuntimeUI = false;
        }
    }
}