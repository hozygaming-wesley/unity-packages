using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using Unity.CodeEditor;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "EditorPathConfig", menuName = "ScriptableObjects/EditorPathConfig", order = 1)]
public class EditorPathConfig : ScriptableObject
{
    public List<string> editorList;
}

public class EditorSwicher : EditorWindow
{
    // private string vsc_path = string.Empty;
    // private string mvs_path = string.Empty;
    public EditorPathConfig editorPathConfig;
    // private string[] editorList = { "Visual Studio Code", "Microsoft Visual Studio" };
    private readonly int labelWidth = 300;
    private int currentSelected = 0;
    [MenuItem("Tools/External Tools Settings")]
    public static void ShowWindow()
    {
        GetWindow<EditorSwicher>("External Tools Settings");
    }

    private void OnGUI()
    {
        // CodeEditor codeEditor = new();

        GUILayout.Label("External Tools Settings", EditorStyles.boldLabel);
        var nowuse = CodeEditor.CurrentEditorInstallation;
        GUILayout.Label($"Now Use {nowuse}", EditorStyles.boldLabel);
        currentSelected = EditorGUILayout.Popup("Select Editor", currentSelected, editorPathConfig.editorList.ToArray());

        // GUILayout.Space(10);

        // GUILayout.BeginHorizontal(GUILayout.Width(10));
        // GUILayout.Label("Visual Studio Code path : ");
        // vsc_path = GUILayout.TextField(vsc_path, GUILayout.Width(labelWidth));
        // GUILayout.EndHorizontal();

        // GUILayout.Space(10);

        // GUILayout.BeginHorizontal(GUILayout.Width(10));
        // GUILayout.Label("Microsoft Visual Studio path : ");
        // mvs_path = GUILayout.TextField(mvs_path, GUILayout.Width(labelWidth));
        // GUILayout.EndHorizontal();

        if (GUILayout.Button("Set External Script Editor"))
        {
            SetExternalScriptEditor();
        }

    }

    private void SetExternalScriptEditor()
    {
        // string scriptEditorPath = currentSelected == 0 ? vsc_path : mvs_path;
        // CodeEditor.SetExternalScriptEditor(scriptEditorPath);
        // Debug.Log("External Script Editor set to: " + scriptEditorPath);
        // if (currentSelected == 0)
        // {
        //     currentSelected = 1;
        // }
        // else
        // {
        //     currentSelected = 0;
        // }
    }
}