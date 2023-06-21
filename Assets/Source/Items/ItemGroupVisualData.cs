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

        public AnimationAttackType AnimationAttackType;

        public bool ShootProjectile = false;
        public ItemVisualData ProjectileData;
    }
}

