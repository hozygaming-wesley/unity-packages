using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptEditorSwicher : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/ScriptEditorSwicher")]
    public static void ShowExample()
    {
        ScriptEditorSwicher wnd = GetWindow<ScriptEditorSwicher>();
        wnd.titleContent = new GUIContent("ScriptEditorSwicher");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        var currentScriptEditor = CodeEditor.CurrentEditorInstallation;

        var currentScriptEditorLabel = root.Q<Label>($"CurrentScriptEditor");

        currentScriptEditorLabel.text = $"Current Script Editor: {currentScriptEditor}";

        // Find the ObjectField named "Setting"
        var settingField = root.Q<ObjectField>("Setting");

        var applyButton = root.Q<Button>("ApplyButton");
        applyButton.clicked += () =>
        {
            Debug.Log("Apply Button Clicked");
            Debug.Log(settingField.value);
            var setting = settingField.value as ScriptEditorSwicherSetting;
            if (setting != null)
            {
                Debug.Log(setting.VisualStudioCode);
                Debug.Log(setting.VisualStudio);

                // Example of changing the script editor to Visual Studio Code
                if (string.IsNullOrEmpty(setting.VisualStudioCode))
                {
                    CodeEditor.SetExternalScriptEditor(setting.VisualStudioCode);
                }
                else if (string.IsNullOrEmpty(setting.VisualStudio))
                {
                    CodeEditor.SetExternalScriptEditor(setting.VisualStudio);
                }
            }
        };
    }
}
