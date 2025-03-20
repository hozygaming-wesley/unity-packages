using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HierarchySpawner
{
    public class HierarchyRenamer : EditorWindow
    {
        [MenuItem("Tools/HierarchyRenamer")]
        public static void ShowExample()
        {
            HierarchyRenamer wnd = GetWindow<HierarchyRenamer>();
            wnd.titleContent = new GUIContent("HierarchyRenamer");
        }
        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Modules/HierarchyKit/Renamer/Editor/HierarchyRenamer.uxml");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Modules/HierarchyKit/Renamer/Editor/HierarchyRenamer.uss");
            root.styleSheets.Add(styleSheet);
            root.Q<Button>("rename").clickable.clicked += () =>
            {
                GameObject[] selected = Selection.gameObjects;
                for (int i = 0; i < selected.Length; i++)
                {
                    selected[i].name = "Object " + i;
                }
            };
        }
    }
}
