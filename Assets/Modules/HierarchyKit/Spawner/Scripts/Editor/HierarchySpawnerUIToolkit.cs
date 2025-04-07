using UnityEditor;
using UnityEngine;
using UnityEngine.UI; // For UnityEngine.UI.Button
using UnityEngine.UIElements; // For UnityEngine.UIElements.Button
using UnityEditor.UIElements;
using System.Collections.Generic;

public class HierarchySpawnerUIToolkit : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private List<PrefabItem> prefabItems = new List<PrefabItem>();

    private class PrefabItem
    {
        public string ButtonText { get; set; }
        public GameObject Prefab { get; set; }
    }

    [MenuItem("Tools/HierarchySpawnerUIToolkit")]
    public static void ShowExample()
    {
        HierarchySpawnerUIToolkit wnd = GetWindow<HierarchySpawnerUIToolkit>();
        wnd.titleContent = new GUIContent("HierarchySpawnerUIToolkit");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);


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

        BindButton("Text", () =>
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

        BindButton("Button", () =>
        {
            CreateGameObject("Button", typeof(RectTransform), go =>
            {
                var image = go.AddComponent<UnityEngine.UI.Image>();
                var button = go.AddComponent<UnityEngine.UI.Button>();
                button.targetGraphic = image;

                var textGO = new GameObject("Text");
                var text = textGO.AddComponent<UnityEngine.UI.Text>();
                text.text = "Button";
                text.fontSize = 20;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.black;

                textGO.transform.SetParent(go.transform);
            });
        });

        BindButton("VLayout", () =>
        {
            CreateGameObject("VLayout", typeof(RectTransform), go =>
            {
                go.AddComponent<VerticalLayoutGroup>();
            });
        });



        var prefabListView = root.Q<ListView>("PrefabListView");
        var addPrefabButton = root.Q<UnityEngine.UIElements.Button>("AddPrefabButton");
        var removePrefabButton = root.Q<UnityEngine.UIElements.Button>("RemovePrefabButton");

        // Configure ListView
        prefabListView.itemsSource = prefabItems;
        prefabListView.makeItem = () =>
        {
            var container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var button = new UnityEngine.UIElements.Button { style = { width = 100, marginRight = 5 } };
            var objectField = new ObjectField { objectType = typeof(GameObject), style = { flexGrow = 1 } };
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
                    Undo.RegisterCreatedObjectUndo(instance, "Spawn Prefab");
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
            prefabItems.Add(new PrefabItem { ButtonText = "Spawn", Prefab = null });
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

        var treeView = root.Q<TreeView>("HierarchyTreeView");

        // Configure TreeView
        var treeItems = new List<TreeViewItemData<string>>
        {
            new TreeViewItemData<string>(1, "Root", new List<TreeViewItemData<string>>
            {
                new TreeViewItemData<string>(2, "Child 1"),
                new TreeViewItemData<string>(3, "Child 2", new List<TreeViewItemData<string>>
                {
                    new TreeViewItemData<string>(4, "Grandchild 1"),
                    new TreeViewItemData<string>(5, "Grandchild 2")
                })
            })
        };

        treeView.SetRootItems(treeItems);
        treeView.makeItem = () => new Label();
        treeView.bindItem = (element, item) =>
        {
            if (element is Label label && treeView.GetItemDataForId<string>((int)item) is string treeItemData)
            {
                label.text = treeItemData;
            }
        };
        treeView.selectionChanged += selectedItems =>
        {
            foreach (var selectedItem in selectedItems)
            {
                if (selectedItem is TreeViewItemData<string> treeItem)
                {
                    Debug.Log($"Selected: {treeItem.data}");
                }
            }
        };
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
        var gameobject = new GameObject(name, componentType);
        if (Selection.activeGameObject != null)
        {
            gameobject.transform.SetParent(Selection.activeGameObject.transform);
        }

        configure?.Invoke(gameobject);

        Undo.RegisterCreatedObjectUndo(gameobject, $"Create {name}");
        Selection.activeGameObject = gameobject;
    }
}
