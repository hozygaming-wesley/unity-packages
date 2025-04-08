using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabListData", menuName = "UISpawner/PrefabListData")]
public class PrefabListData : ScriptableObject
{
    [Serializable]
    public class PrefabItemData
    {
        public string ButtonText;
        public GameObject Prefab;
    }

    public List<PrefabItemData> prefabItems = new List<PrefabItemData>();

    public event Action OnDataChanged;

    public void NotifyDataChanged()
    {
        OnDataChanged?.Invoke();
    }
}
