using UnityEngine;
using UnityEditor;

namespace HotKeyExtensions
{

    public class CustomShortcuts : Editor
    {
        [MenuItem("Custom Shortcuts/Do Something _%#D")] // Ctrl + Shift + D
        static void DoSomething()
        {
            Debug.Log("Custom shortcut executed!");
        }
    }
}
