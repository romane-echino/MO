using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MO.Character.BodyAspect;

namespace MO.Item
{
    [CreateAssetMenu(fileName = "ItemVisualData", menuName = "Data/Items/VisualData", order = 0)]
    public class ItemVisualData : ScriptableObject
    {
        public string Id;
        public Sprite Sprite;
        public BodyPartAnchorType AnchorType;
        public int LayerOrder;
    }
}

