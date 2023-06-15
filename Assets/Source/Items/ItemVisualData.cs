using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MO.Character.BodyAspect;
using MO.Character;

namespace MO.Item
{
    [CreateAssetMenu(fileName = "ItemVisualData", menuName = "Data/Items/VisualData", order = 0)]
    public class ItemVisualData : ScriptableObject
    {
        public string Id;
        public Sprite Sprite;
        public BodyPartAnchorType AnchorType;
        public bool UseSpecificLayerOrder = false;
        public int LayerOrder;
        
        public List<BodyPartType> VisibleBodyParts = new List<BodyPartType>();
        public AnimationAttackType AnimationAttackType;

        public bool ShootProjectile = false;
        public string ProjectileId;
    }
}

