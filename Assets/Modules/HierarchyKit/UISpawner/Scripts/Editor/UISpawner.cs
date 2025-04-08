using UnityEditor;
using UnityEngine;
using UnityEngine.UI; // For UnityEngine.UI.Button
using UnityEngine.UIElements; // For UnityEngine.UIElements.Button
using UnityEditor.UIElements;
using System.Collections.Generic;
using TMPro;
using System.IO;
using System.Linq; 
public class UISpawner : EditorWindow
{
    const string _uguiPrefab_PackagePath = "Packages/com.untiy.tools.hierarchykit/UISpawner/Resources/Prefab/UGUI/";
    const string _uguiPrefab_AssetPath = "Assets/Modules/HierarchyKit/UISpawner/Resources/Prefab/";
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private List<PrefabItem> prefabItems = new List<PrefabItem>();
    private PrefabListData prefabListData;

    private class PrefabItem
    {
        public string ButtonText { get; set; }
        public GameObject Prefab { get; set; }
    }

    [MenuItem("Tools/UISpawner")]
    public static void ShowExample()
    {
        UISpawner wnd = GetWindow<UISpawner>();
        wnd.titleContent = new GUIContent("UISpawner");
    }

    private TextField prefixField;
    private TextField separatorField;
    private TextField suffixField;

    public void CreateGUI()
    {
        if (m_VisualTreeAsset == null)
        {
            Debug.LogError("VisualTreeAsset is not assigned. Please assign the UISpawner.uxml file to m_VisualTreeAsset.");
            return;
        }

        VisualElement root = rootVisualElement;
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        // 確保 PrefabListView 存在
        var prefabListView = root.Q<ListView>("PrefabListView");
        if (prefabListView == null)
        {
            Debug.LogError("PrefabListView not found in the UI hierarchy. Please check the UXML file.");
            return;
        }

        // 獲取命名規則的 TextField
        prefixField = root.Q<TextField>("Prefix");
        separatorField = root.Q<TextField>("Separator");
        suffixField = root.Q<TextField>("Suffix");

        // 調整 TextField 樣式以支持縮小時輸入
        prefixField.style.minWidth = 50; // 設置最小寬度，確保輸入框始終可見
        prefixField.style.flexGrow = 1; // 允許彈性擴展
        prefixField.style.flexShrink = 1; // 允許縮小
        prefixField.style.overflow = Overflow.Visible; // 啟用滾動條以防止內容被裁剪

        separatorField.style.minWidth = 50;
        separatorField.style.flexGrow = 1;
        separatorField.style.flexShrink = 1;
        separatorField.style.overflow = Overflow.Visible;

        suffixField.style.minWidth = 50;
        suffixField.style.flexGrow = 1;
        suffixField.style.flexShrink = 1;
        suffixField.style.overflow = Overflow.Visible;

        // Bind buttons to their respective actions
        BindButton("Canvas", () =>
        {
            CreateGameObject("Canvas", typeof(RectTransform), gameobject =>
            {
                var canvas = gameobject.AddComponent<Canvas>();
                var scaler = gameobject.AddComponent<CanvasScaler>();
                var raycaster = gameobject.AddComponent<GraphicRaycaster>();

                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = Camera.main;
                canvas.pixelPerfect = true;

                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1280, 720);
            });
        });

        BindButton("Image", () =>
        {
            CreateGameObject("Image", typeof(RectTransform), go =>
            {
                go.AddComponent<UnityEngine.UI.Image>();
            });
        });

        BindButton("RawImage", () =>
        {
            CreateGameObject("RawImage", typeof(RectTransform), go =>
            {
                go.AddComponent<RawImage>();
            });
        });

        BindButton("Sprite", () =>
        {
            CreateGameObject("Sprite", typeof(RectTransform), go =>
            {
                go.AddComponent<SpriteRenderer>();
            });
        });

        BindButton("Legacy_Text", () =>
        {
            CreateGameObject("Text", typeof(RectTransform), go =>
            {
                var text = go.AddComponent<UnityEngine.UI.Text>();
                text.text = "Default Text";
                text.fontSize = 26;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.black;
            });
        });

        BindButton("TMP_Text", () =>
        {
            CreateGameObject("Text", typeof(RectTransform), go =>
            {
                var text = go.AddComponent<TextMeshProUGUI>();
                text.text = "Default Text";
                text.fontSize = 26;
                text.alignment = TextAlignmentOptions.Center;

                if (TMP_Settings.defaultFontAsset != null)
                {
                    text.font = TMP_Settings.defaultFontAsset;
                    text.fontMaterial = TMP_Settings.defaultFontAsset.material;
                }
                else
                {
                    Debug.LogWarning("TMP_Settings.defaultFontAsset is null. Please assign a default font asset in TMP Settings.");
                }

                text.color = Color.black;
            });
        });

        var addPrefabButton = root.Q<UnityEngine.UIElements.Button>("AddPrefabButton");
        var removePrefabButton = root.Q<UnityEngine.UIElements.Button>("RemovePrefabButton");

        // Configure ListView
        prefabListView.itemsSource = prefabItems;
        prefabListView.makeItem = () =>
        {
            var container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var button = new UnityEngine.UIElements.Button { style = { width = 25, marginRight = 5 } };
            var objectField = new ObjectField
            {
                objectType = typeof(GameObject),
                style =
                {
                    flexGrow = 1,
                    alignItems = Align.Center, // 垂直居中
                    justifyContent = Justify.Center // 水平居中
                }
            };
            container.Add(button);
            container.Add(objectField);
            return container;
        };
        prefabListView.bindItem = (element, index) =>
        {
            var container = (VisualElement)element;
            var button = (UnityEngine.UIElements.Button)container.ElementAt(0);
            var objectField = (ObjectField)container.ElementAt(1);

            var item = prefabItems[index];
            button.text = item.ButtonText ?? $"Spawn {index + 1}";
            objectField.value = item.Prefab;

            button.clicked += () =>
            {
                if (objectField.value is GameObject prefab)
                {
                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                    // 將生成的 Prefab 轉換為普通 GameObject
                    PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.UserAction);

                    // 將生成的 GameObject 放置在當前選擇的 GameObject 底下
                    if (Selection.activeGameObject != null)
                    {
                        instance.transform.SetParent(Selection.activeGameObject.transform);
                        instance.transform.localPosition = Vector3.zero; // 將位置設為父物件的正中央
                    }

                    Undo.RegisterCreatedObjectUndo(instance, "Spawn Prefab");

                    // 選取生成的 GameObject
                    Selection.activeGameObject = instance;
                }
            };

            objectField.RegisterValueChangedCallback(evt =>
            {
                item.Prefab = evt.newValue as GameObject;
            });
        };
        prefabListView.fixedItemHeight = 30;

        // Add item
        addPrefabButton.clicked += () =>
        {
            prefabItems.Add(new PrefabItem { ButtonText = "+", Prefab = null });
            prefabListView.Rebuild();
        };

        // Remove item
        removePrefabButton.clicked += () =>
        {
            if (prefabItems.Count > 0)
            {
                prefabItems.RemoveAt(prefabItems.Count - 1);
                prefabListView.Rebuild();
            }
        };

        var setDefaultPrefabsButton = new UnityEngine.UIElements.Button(() =>
        {
            // 嘗試動態查找 Prefab 資料夾
            string prefabPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:Folder Prefab")
                .FirstOrDefault(guid => AssetDatabase.GUIDToAssetPath(guid).EndsWith("Prefab")));

            if (string.IsNullOrEmpty(prefabPath) || !Directory.Exists(prefabPath))
            {
                Debug.LogError($"Prefab path does not exist: {prefabPath}");
                return;
            }

            string[] prefabFiles = Directory.GetFiles(prefabPath, "*.prefab");

            prefabItems.Clear();
            foreach (var prefabFile in prefabFiles)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabFile);
                if (prefab != null)
                {
                    prefabItems.Add(new PrefabItem
                    {
                        ButtonText = "+",
                        Prefab = prefab
                    });
                }
            }
            prefabListView.Rebuild();
        })
        {
            text = "Set Default Prefabs"
        };

        root.Q<VisualElement>("PrefabListControls").Add(setDefaultPrefabsButton);
    }
    private void BindButton(string buttonName, System.Action action)
    {
        var button = rootVisualElement.Q<UnityEngine.UIElements.Button>(buttonName);
        if (button != null)
        {
            button.clicked += () => action.Invoke();
        }
    }

    private void CreateGameObject(string name, System.Type componentType, System.Action<GameObject> configure = null)
    {
        // 使用命名規則生成名稱
        string prefix = prefixField?.value ?? string.Empty;
        string separator = separatorField?.value ?? string.Empty;
        string suffix = suffixField?.value ?? string.Empty;
        string objectName = $"{prefix}{separator}{name}{separator}{suffix}";

        var gameobject = new GameObject(objectName, componentType);
        if (Selection.activeGameObject != null)
        {
            gameobject.transform.SetParent(Selection.activeGameObject.transform);
        }

        configure?.Invoke(gameobject);

        Undo.RegisterCreatedObjectUndo(gameobject, $"Create {objectName}");
        Selection.activeGameObject = gameobject;
    }

    private void SavePrefabListData()
    {
        if (prefabListData != null)
        {
            EditorUtility.SetDirty(prefabListData);
            AssetDatabase.SaveAssets();
        }
    }

    private void BindDataToListView()
    {
        var prefabListView = rootVisualElement.Q<ListView>("PrefabListView");

        if (prefabListView == null)
        {
            Debug.LogError("PrefabListView not found in the UI hierarchy. Please check the UXML file.");
            return;
        }

        if (prefabListData == null)
        {
            Debug.LogError("PrefabListData is null. Ensure it is properly initialized in EnsureConfigExists.");
            return;
        }

        // 設定 ListView 的資料來源
        prefabListView.itemsSource = prefabListData.prefabItems;

        // 設定 ListView 的項目生成邏輯
        prefabListView.makeItem = () =>
        {
            var container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var button = new UnityEngine.UIElements.Button { style = { width = 25, marginRight = 5 } };
            var objectField = new ObjectField
            {
                objectType = typeof(GameObject),
                style =
                {
                    flexGrow = 1,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                }
            };
            container.Add(button);
            container.Add(objectField);
            return container;
        };

        // 設定 ListView 的項目綁定邏輯
        prefabListView.bindItem = (element, index) =>
        {
            var container = (VisualElement)element;
            var button = (UnityEngine.UIElements.Button)container.ElementAt(0);
            var objectField = (ObjectField)container.ElementAt(1);

            var item = prefabListData.prefabItems[index];
            button.text = item.ButtonText ?? $"Spawn {index + 1}";
            objectField.value = item.Prefab;

            button.clicked += () =>
            {
                if (objectField.value is GameObject prefab)
                {
                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                    // 將生成的 Prefab 轉換為普通 GameObject
                    PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.UserAction);

                    // 將生成的 GameObject 放置在當前選擇的 GameObject 底下
                    if (Selection.activeGameObject != null)
                    {
                        instance.transform.SetParent(Selection.activeGameObject.transform);
                        instance.transform.localPosition = Vector3.zero;
                    }

                    Undo.RegisterCreatedObjectUndo(instance, "Spawn Prefab");
                    Selection.activeGameObject = instance;
                }
            };

            objectField.RegisterValueChangedCallback(evt =>
            {
                item.Prefab = evt.newValue as GameObject;
                prefabListData.NotifyDataChanged(); // 通知資料變更
                SavePrefabListData();
            });
        };

        prefabListView.fixedItemHeight = 30;

        // 監聽資料變更
        prefabListData.OnDataChanged += () =>
        {
            prefabListView.Rebuild();
        };
    }

    private void AddPrefabItem()
    {
        prefabListData.prefabItems.Add(new PrefabListData.PrefabItemData { ButtonText = "+", Prefab = null });
        prefabListData.NotifyDataChanged();
        SavePrefabListData();
    }

    private void RemovePrefabItem()
    {
        if (prefabListData.prefabItems.Count > 0)
        {
            prefabListData.prefabItems.RemoveAt(prefabListData.prefabItems.Count - 1);
            prefabListData.NotifyDataChanged();
            SavePrefabListData();
        }
    }

    private void EnsureConfigExists()
    {
        const string ConfigAssetPath = "Assets/Modules/HierarchyKit/UISpawner/Resources/PrefabListData.asset";

        // 嘗試加載資產
        prefabListData = AssetDatabase.LoadAssetAtPath<PrefabListData>(ConfigAssetPath);

        // 如果資產不存在，則創建新的資產
        if (prefabListData == null)
        {
            prefabListData = ScriptableObject.CreateInstance<PrefabListData>();
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigAssetPath));
            AssetDatabase.CreateAsset(prefabListData, ConfigAssetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"Created new PrefabListData at {ConfigAssetPath}");
        }
    }

    private void OnEnable()
    {
        EnsureConfigExists();
        BindDataToListView();
    }
}
