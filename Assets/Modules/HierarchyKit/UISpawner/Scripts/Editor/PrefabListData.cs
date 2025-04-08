using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabListData", menuName = "UISpawner/PrefabListData")]
public class PrefabListData : ScriptableObject
{
    [System.Serializable]
    public class PrefabItemData
    {
        public string ButtonText;
        public GameObject Prefab;
    }

    public List<PrefabItemData> prefabItems = new List<PrefabItemData>();
}