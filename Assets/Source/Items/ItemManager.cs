using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MO.Item
{
    public class ItemManager : MonoBehaviour {
        
        [SerializeField]
        private List<ItemVisualData> itemVisualDatas = new List<ItemVisualData>();
        public IEnumerable<ItemVisualData> ItemVisualDatas => itemVisualDatas;

        private Dictionary<string, ItemVisualData> ItemPerKey = new Dictionary<string, ItemVisualData>();

        private void Awake() {
            foreach(var item in ItemVisualDatas)
            {
                ItemPerKey.Add(item.Id, item);
            }
        }

        public ItemVisualData GetItemVisualData(string id) => ItemPerKey[id];

    }
}

