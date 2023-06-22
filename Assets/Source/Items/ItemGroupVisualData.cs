using System.Collections.Generic;
using UnityEngine;
using MO.Character;

namespace MO.Item
{
    [CreateAssetMenu(fileName ="ItemVisualGroupData", menuName = "Data/Items/ItemVisualGroupData", order = 0)]
    public class ItemGroupVisualData: ScriptableObject
    {
        public string Id;

        public List<ItemVisualData> items = new List<ItemVisualData>();

        // If we need to override presentation angles for some object group ??
        //public List<float> PresentationAngles = new List<float>() { };

        public AnimationAttackType AnimationAttackType;

        public bool ShootProjectile = false;
        public ItemVisualData ProjectileData;
    }
}

