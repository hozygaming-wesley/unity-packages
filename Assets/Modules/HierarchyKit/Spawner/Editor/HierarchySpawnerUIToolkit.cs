using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HierarchySpawnerUIToolkit : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/HierarchySpawnerUIToolkit")]
    public static void ShowExample()
    {
        HierarchySpawnerUIToolkit wnd = GetWindow<HierarchySpawnerUIToolkit>();
        wnd.titleContent = new GUIContent("HierarchySpawnerUIToolkit");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
