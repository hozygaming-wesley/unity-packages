#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using TMPro;


#endif
namespace Wesley4121
{
#if UNITY_EDITOR
    public class UGUI_Spawner : EditorWindow
    {
        const int _width = 57;
        const int _hight = 25;
        [MenuItem("Wesley4121/UGUI_Spawner")]
        static void Init()
        {
            var inspWndType = typeof(SceneView);
            var window = GetWindow<UGUI_Spawner>("UGUI_Spawner", inspWndType);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            if (GUILayout.Button("Canvas", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject("Canvas_", typeof(RectTransform));
                var gocv = go.AddComponent<Canvas>();
                var gocvs = go.AddComponent<CanvasScaler>();
                var gogr = go.AddComponent<GraphicRaycaster>();

                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                go.layer = 5;
                //Canvas設定
                gocv.renderMode = RenderMode.ScreenSpaceCamera;
                gocv.planeDistance = 1;
                gocv.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                gocv.sortingLayerName = "UI";
                gocv.pixelPerfect = true;
                //CanvasScaler設定
                gocvs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                gocvs.referenceResolution = new Vector2(1920, 1080);
                gocvs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeObject = go;
            }

            if (GUILayout.Button("GO", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                bool? hasit = Selection.activeGameObject?.TryGetComponent(out RectTransform rect);
                Type type = hasit.HasValue ? typeof(RectTransform) : typeof(Transform);
                var go = new GameObject("GameObject_", type ?? typeof(Transform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }




            if (GUILayout.Button("Image", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject("Image_", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                var img = go.AddComponent<UnityEngine.UI.Image>();
                img.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }




            if (GUILayout.Button("RawI", GUILayout.Width(_width), GUILayout.Height(_hight), GUILayout.ExpandWidth(false)))
            {
                var go = new GameObject("RawImage_", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                RawImage image = go.AddComponent<RawImage>();
                image.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeObject = go;
            }

            if (GUILayout.Button("Text", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject("Text_");
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);
                UnityEngine.UI.Text text = go.AddComponent<UnityEngine.UI.Text>();
                text.rectTransform.sizeDelta = new Vector2(100, 100);
                // text.font = AssetDatabase.LoadAssetAtPath<Font>(
                //     "Assets/6.Front/FrontType/BaseType/fusion-pixel-monospaced.otf");
                text.text = "default";
                text.fontSize = 26;
                text.alignment = TextAnchor.MiddleCenter;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.color = Color.black;
                text.lineSpacing = 0.8f;
                text.supportRichText = false;
                text.raycastTarget = false;

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeObject = go;
            }
            if (GUILayout.Button("Sprite", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {

                bool? hasit = Selection.activeGameObject?.TryGetComponent(out RectTransform rect);
                var go = new GameObject("Sprite_");
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lv = go.AddComponent<SpriteRenderer>();

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }


            EditorGUILayout.EndHorizontal();


            // ============================================================================================================================

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));




            // if (GUILayout.Button("TMP", GUILayout.Width(_width), GUILayout.Height(_hight)))
            // {
            //     var go = new GameObject("TMPText_", typeof(RectTransform));
            //     GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

            //     var tmp = go.AddComponent<TextMeshProUGUI>();
            //     tmp.color = $"#FFE3B8".ToColor();
            //     tmp.text = "default";

            //     tmp.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            //         "Assets/6.Front/FrontType/TMPType/fusion-pixel-monospaced SDF_tw.asset");
            //     Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
            //     Selection.activeGameObject = go;
            // }

            if (GUILayout.Button("VLayout", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject("VLayout_", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lv = go.AddComponent<VerticalLayoutGroup>();

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("HLayout", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {
                var go = new GameObject("HLayout_", typeof(RectTransform));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                var lh = go.AddComponent<HorizontalLayoutGroup>();

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }





            if (GUILayout.Button("VSlider", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Packages/com.wesley4121.tool/Prefab/VSlider.prefab"));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);



                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }
            if (GUILayout.Button("HSlider", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Packages/com.wesley4121.tool/Prefab/HSlider.prefab"));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }
            if (GUILayout.Button("VSR", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Packages/com.wesley4121.tool/Prefab/VScrollRect.prefab"));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }
            if (GUILayout.Button("HSR", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Packages/com.wesley4121.tool/Prefab/HScrollRect.prefab"));
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            if (GUILayout.Button("Button", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                // var go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(
                //   "Assets/Plugins/0.Wesley/Prefab/Button.prefab"));
                var go = new GameObject("Button_", typeof(RectTransform));
                var gort = go.GetComponent<RectTransform>();
                gort.sizeDelta = new Vector2(160, 30);
                var image = go.AddComponent<Image>();
                image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Resources/unity_builtin_extra/UISprite");
                var btn = go.AddComponent<Button>();
                btn.targetGraphic = image;
                var Btntext = new GameObject("BtnText_");
                var text = Btntext.AddComponent<Text>();
                var textrt = go.GetComponent<RectTransform>();
                textrt.sizeDelta = new Vector2(160, 30);
                text.text = "BtnText";
                text.fontSize = 30;
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                GameObjectUtility.SetParentAndAlign(Btntext, go);
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }

            if (GUILayout.Button("TMPBtn", GUILayout.Width(_width), GUILayout.Height(_hight)))
            {


                var go = new GameObject("Button_", typeof(RectTransform));
                var gort = go.GetComponent<RectTransform>();
                gort.sizeDelta = new Vector2(160, 30);
                var image = go.AddComponent<Image>();
                image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Resources/unity_builtin_extra/UISprite");
                var btn = go.AddComponent<Button>();
                btn.targetGraphic = image;
                var Btntext = new GameObject("BtnText_");
                var text = Btntext.AddComponent<TextMeshProUGUI>();
                var textrt = go.GetComponent<RectTransform>();
                textrt.sizeDelta = new Vector2(160, 30);
                text.text = "BtnText";
                text.fontSize = 30;
                text.color = Color.black;
                text.alignment = TextAlignmentOptions.Center;

                GameObjectUtility.SetParentAndAlign(Btntext, go);
                GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

                Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
                Selection.activeGameObject = go;
            }

            EditorGUILayout.EndHorizontal();






        }
    }
#endif
}
