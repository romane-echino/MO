using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MO.Utils;

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

    }
}

