using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class CashCleaner : MonoBehaviour
    {
        [MenuItem("Window/Clean Cash")]
        private static void Clean()
        {
            PlayerPrefs.DeleteAll();
        } 
    }
}