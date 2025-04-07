using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using TMPro;

namespace HierarchySpawner
{
    public class HierarchySpawner : EditorWindow
    {
        const int _width = 57;
        const int _hight = 25;
        const string _uguiPrefab_AssetsPath = "Modules/HierarchySpawner/Prefab/UGUI/";
        const string _uguiPrefab_PackagePath = "Packages/com.wesley4121.tools/Spawner/Prefab/UGUI/";
        static private bool isPackage = true;

        private string prefix = "Prefix";
        private string suffix = "Suffix";
        private string separator = "_";

        [MenuItem("Tools/HierarchySpawner")]
        static void Init()
        {
            HierarchySpawner wnd = GetWindow<HierarchySpawner>();
            wnd.titleContent = new GUIContent("HierarchyRenamer");

            isPackage = AssetDatabase.IsValidFolder("Packages/com.wesley4121.tools");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefix:", GUILayout.Width(50));
            prefix = EditorGUILayout.TextField(prefix, GUILayout.Width(100));
            EditorGUILayout.LabelField("Separator:", GUILayout.Width(70));
            separator = EditorGUILayout.TextField(separator, GUILayout.Width(50));
            EditorGUILayout.LabelField("Suffix:", GUILayout.Width(50));
            suffix = EditorGUILayout.TextField(suffix, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Canvas", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject($"{prefix}{separator}Canvas{separator}{suffix}", typeof(RectTransform));
                var gocv = go.AddComponent<Canvas>();
                var gocvs = go.AddComponent<CanvasScaler>();
                var gogr = go.AddComponent<GraphicRaycaster>();

                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                go.layer = 5;
                // Canvas settings
                gocv.renderMode = RenderMode.ScreenSpaceCamera;
                gocv.planeDistance = 1;
                gocv.worldCamera = Camera.main;
                gocv.sortingLayerName = "UI";
                gocv.pixelPerfect = true;
                // CanvasScaler settings
                gocvs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                gocvs.referenceResolution = new Vector2(1280, 720);
                gocvs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeObject = go;
            }

            if (GUILayout.Button("Group", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                bool? hasit = Selection.activeGameObject?.TryGetComponent(out RectTransform rect);
                Type type = hasit.HasValue ? typeof(RectTransform) : typeof(Transform);
                var go = new GameObject($"{prefix}{separator}Group{separator}{suffix}", type ?? typeof(Transform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("Image", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject($"{prefix}{separator}Image{separator}{suffix}", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                var img = go.AddComponent<UnityEngine.UI.Image>();
                img.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("RawI", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject($"{prefix}{separator}RawImage{separator}{suffix}", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                RawImage image = go.AddComponent<RawImage>();
                image.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeObject = go;
            }

            if (GUILayout.Button("Text", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}Text{separator}{suffix}");
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                UnityEngine.UI.Text text = go.AddComponent<UnityEngine.UI.Text>();
                text.rectTransform.sizeDelta = new Vector2(100, 100);
                text.text = "default";
                text.fontSize = 26;
                text.alignment = TextAnchor.MiddleCenter;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.color = Color.black;
                text.lineSpacing = 0.8f;
                text.supportRichText = false;
                text.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeObject = go;
            }

            if (GUILayout.Button("Sprite", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                bool? hasit = Selection.activeGameObject?.TryGetComponent(out RectTransform rect);
                var go = new GameObject($"{prefix}{separator}Sprite{separator}{suffix}");
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lv = go.AddComponent<SpriteRenderer>();

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            EditorGUILayout.EndHorizontal();

            // ============================================================================================================================

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("VLayout", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}VLayout{separator}{suffix}", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lv = go.AddComponent<VerticalLayoutGroup>();

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("HLayout", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}HLayout{separator}{suffix}", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lh = go.AddComponent<HorizontalLayoutGroup>();

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("VSlider", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    isPackage ? _uguiPrefab_PackagePath + "VSlider.prefab" : _uguiPrefab_AssetsPath + "VSlider.prefab"
                ));
                go.name = $"{prefix}{separator}VSlider{separator}{suffix}";
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("HSlider", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    isPackage ? _uguiPrefab_PackagePath + "HSlider.prefab" : _uguiPrefab_AssetsPath + "HSlider.prefab"
                ));
                go.name = $"{prefix}{separator}HSlider{separator}{suffix}";
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("VSR", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    isPackage ? _uguiPrefab_PackagePath + "VScrollRect.prefab" : _uguiPrefab_AssetsPath + "VScrollRect.prefab"
                ));
                go.name = $"{prefix}{separator}VScrollRect{separator}{suffix}";
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("HSR", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    isPackage ? _uguiPrefab_PackagePath + "HScrollRect.prefab" : _uguiPrefab_AssetsPath + "HScrollRect.prefab"
                ));
                go.name = $"{prefix}{separator}HScrollRect{separator}{suffix}";
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Button", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}Button{separator}{suffix}", typeof(RectTransform));
                var gort = go.GetComponent<RectTransform>();
                gort.sizeDelta = new Vector2(160, 30);
                var image = go.AddComponent<Image>();
                var btn = go.AddComponent<Button>();
                btn.targetGraphic = image;
                var Btntext = new GameObject($"{prefix}{separator}Text{separator}{suffix}");
                var text = Btntext.AddComponent<Text>();
                var textrt = go.GetComponent<RectTransform>();
                textrt.sizeDelta = new Vector2(160, 30);
                text.text = "Text";
                text.fontSize = 30;
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                GameObjectUtility.SetParentAndAlign(Btntext, go);
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("TMP", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}Text{separator}{suffix}", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                var tmp = go.AddComponent<TextMeshProUGUI>();
                tmp.raycastTarget = false;
                tmp.richText = false;
                tmp.maskable = false;
                tmp.parseCtrlCharacters = false;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.overflowMode = TextOverflowModes.Truncate;
                tmp.text = "default";
                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("TMPBtn", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject($"{prefix}{separator}Button{separator}{suffix}", typeof(RectTransform));
                var gort = go.GetComponent<RectTransform>();
                gort.sizeDelta = new Vector2(160, 30);
                var image = go.AddComponent<Image>();
                var btn = go.AddComponent<Button>();
                btn.targetGraphic = image;
                var Btntext = new GameObject($"{prefix}{separator}Text{separator}{suffix}");
                var text = Btntext.AddComponent<TextMeshProUGUI>();
                var textrt = go.GetComponent<RectTransform>();
                textrt.sizeDelta = new Vector2(160, 30);
                text.text = "Text";
                text.fontSize = 30;
                text.color = Color.black;
                text.alignment = TextAlignmentOptions.Center;

                GameObjectUtility.SetParentAndAlign(Btntext, go);
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                Selection.activeGameObject = go;
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
