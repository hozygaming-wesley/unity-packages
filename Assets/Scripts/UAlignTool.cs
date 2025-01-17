#if UNITY_EDITOR



using UnityEditor;
using UnityEngine;
namespace Wesley.Tool
{
    public class UAlignTool : EditorWindow
    {
        int width = 65;
        int hight = 25;
        [MenuItem("Wesley/AlignTool")]

        private static void OpenWindow()
        {
            GetWindow<UAlignTool>().Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("SetParentAlign", GUILayout.Width(100), GUILayout.Height(hight)))
            {
                SetParentAlign();
            }
            if (GUILayout.Button("AlignY", GUILayout.Width(width), GUILayout.Height(hight)))
            {
                Align(type.PosY);
            }
            if (GUILayout.Button("AlignX", GUILayout.Width(width), GUILayout.Height(hight)))
            {
                Align(type.PosX);
            }
            if (GUILayout.Button("AlignBoth", GUILayout.Width(75), GUILayout.Height(hight)))
            {
                Align(type.both);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            if (Selection.activeGameObject | GUI.changed)
            {
                string alignGO = $"AlignObject \"{Selection.gameObjects[0].gameObject.name}\"" ?? "NoObject!!";
                Selection.selectionChanged += () =>
                {
                    try { alignGO = $"AlignObject \"{Selection.gameObjects[0].gameObject.name}\""; } catch (System.Exception) { }
                };
                EditorGUILayout.LabelField(alignGO);
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var display = $"{i} {Selection.gameObjects[i].gameObject.name} is seleted!" ?? "NoObject!!";
                    EditorGUILayout.LabelField(display);
                }
            }

            EditorGUILayout.EndVertical();

            void SetParentAlign()
            {
                var alignRT = Selection.gameObjects[0]?.GetComponent<RectTransform>();


                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var GORT = Selection.gameObjects[i].GetComponent<RectTransform>();
                    GameObjectUtility.SetParentAndAlign(Selection.gameObjects[i], alignRT.gameObject);
                    Undo.SetTransformParent(GORT, alignRT, $"SetAlign");

                }
            }

            void Align(type type)
            {

                var parentRT = Selection.gameObjects[0]?.GetComponent<RectTransform>();


                switch (type)
                {
                    case type.both:
                        for (int i = 1; i < Selection.gameObjects.Length; i++)
                        {
                            var childRT = Selection.gameObjects[i]?.GetComponent<RectTransform>();
                            childRT.localPosition = new Vector3(parentRT.localPosition.x, parentRT.localPosition.y, parentRT.localPosition.z);
                            Undo.RegisterFullObjectHierarchyUndo(Selection.gameObjects[i], "Align");
                        }
                        break;
                    case type.PosX:
                        for (int i = 1; i < Selection.gameObjects.Length; i++)
                        {
                            var childRT = Selection.gameObjects[i]?.GetComponent<RectTransform>();
                            childRT.localPosition = new Vector3(parentRT.localPosition.x, childRT.localPosition.y, childRT.localPosition.z);
                            Undo.RegisterFullObjectHierarchyUndo(Selection.gameObjects[i], "Align");
                        }
                        break;
                    case type.PosY:
                        for (int i = 1; i < Selection.gameObjects.Length; i++)
                        {
                            var childRT = Selection.gameObjects[i]?.GetComponent<RectTransform>();
                            childRT.localPosition = new Vector3(childRT.localPosition.x, parentRT.localPosition.y, childRT.localPosition.z);
                            Undo.RegisterFullObjectHierarchyUndo(Selection.gameObjects[i], "Align");
                        }
                        break;

                }

            }



        }

        public enum type
        {
            both, PosY, PosX
        }
    }
}
#endif
