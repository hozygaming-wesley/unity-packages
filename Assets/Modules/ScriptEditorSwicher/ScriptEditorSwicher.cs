using System.Collections.Generic;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptEditorSwicher : EditorWindow {
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/ScriptEditorSwicher")]
    public static void ShowExample() {
        ScriptEditorSwicher wnd = GetWindow<ScriptEditorSwicher>();
        wnd.titleContent = new GUIContent("ScriptEditorSwicher");
    }

    public void CreateGUI() {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        // Get available script editors
        var foundScriptEditors = CodeEditor.Editor.GetFoundScriptEditorPaths();
        var availableEditorsPath = foundScriptEditors.Select(pair => pair.Key).ToList();
        var availableEditors = foundScriptEditors.Select(pair => pair.Value).ToList();

        // Setup current script editor label
        var currentScriptEditorPath = CodeEditor.CurrentEditorInstallation;
        var currentScriptEditorLabel = root.Q<Label>("CurrentScriptEditor");



        // Determine the editor name based on the current script editor path
        string editorName = foundScriptEditors.FirstOrDefault(pair => currentScriptEditorPath.Contains(pair.Key)).Value ?? "Unknown Editor";
        currentScriptEditorLabel.text = $"Current Script Editor: {editorName}";

        // Setup dropdown menu
        var dropdown = root.Q<DropdownField>("ScriptEditorDropdown");
        dropdown.choices = availableEditors;
        dropdown.index = availableEditorsPath.IndexOf(currentScriptEditorPath);

        dropdown.RegisterValueChangedCallback(evt => {
            Debug.Log("Dropdown value changed to: " + evt.newValue);
            // Get event index
            int index = dropdown.index;
            // Set new script editor
            CodeEditor.SetExternalScriptEditor(availableEditorsPath[index]);
            // Refresh current script editor label
            currentScriptEditorPath = availableEditorsPath[index];
            editorName = foundScriptEditors.FirstOrDefault(pair => currentScriptEditorPath.Contains(pair.Key)).Value ?? "Unknown Editor";
            currentScriptEditorLabel.text = $"Current Script Editor: {editorName}";
        });

        // Remove apply button if not used
        var applyButton = root.Q<Button>("ApplyButton");
        if (applyButton != null) {
            applyButton.clicked += () => {
                Debug.Log("Apply Button Clicked");
            };
        }
    }
}