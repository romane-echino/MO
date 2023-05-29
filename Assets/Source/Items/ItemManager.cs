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
        private List<ItemVisualData> itemVisualDatas = new List<ItemVisualData>();
        public IEnumerable<ItemVisualData> ItemVisualDatas => itemVisualDatas;

        private Dictionary<string, ItemVisualData> ItemPerKey = new Dictionary<string, ItemVisualData>();

        protected override void Awake() {
            base.Awake();
            foreach(var item in ItemVisualDatas)
            {
                ItemPerKey.Add(item.Id, item);
            }
        }

        public ItemVisualData GetItemVisualData(string id) => ItemPerKey[id];

#if UNITY_EDITOR
        [ContextMenu("Find all item visual data")]
        private void FindAllItemOnEditor()
        {
            var datas = GetAllInstances<ItemVisualData>();
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

