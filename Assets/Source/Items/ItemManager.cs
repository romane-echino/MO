using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MO.Utils;
using System.Linq;

namespace MO.Item
{
    public class ItemManager : Singleton<ItemManager> {
        
        [SerializeField]
        private List<ItemGroupVisualData> itemVisualDatas = new List<ItemGroupVisualData>();
        public IEnumerable<ItemGroupVisualData> ItemVisualDatas => itemVisualDatas;

        private Dictionary<string, ItemGroupVisualData> ItemPerKey = new Dictionary<string, ItemGroupVisualData>();

        protected override void Awake() {
            base.Awake();
            foreach(var item in ItemVisualDatas)
            {
                ItemPerKey.Add(item.Id, item);
            }
        }

        public ItemGroupVisualData GetItemVisualData(string id) => ItemPerKey[id];

#if UNITY_EDITOR
        [ContextMenu("Find all item visual data")]
        private void FindAllItemOnEditor()
        {
            var datas = GetAllInstances<ItemGroupVisualData>();
            itemVisualDatas = datas.ToList();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;

        }
#endif
    }
}

