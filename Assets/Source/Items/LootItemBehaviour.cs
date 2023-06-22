using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MO.Item
{
    public class LootItemBehaviour : MonoBehaviour
    {
        public List<float> anglePerItem = new List<float>() { -40f, 40f, 90f, -90f };

        [SerializeField]
        private List<CenteredSpriteBehaviour> centeredSprites = new List<CenteredSpriteBehaviour>();

        public void SetItem(ItemGroupVisualData itemGroup)
        {
            foreach (var sprite in centeredSprites)
            {
                sprite.Reset();
            }
            for (int i = 0; i < itemGroup.items.Count; i++)
            {
                if (i >= centeredSprites.Count)
                {
                    break;
                }
                centeredSprites[i].SetSprite(itemGroup.items[i].Sprite, itemGroup.items[i].PresentationAngle);
            }
        }

#if UNITY_EDITOR

        public ItemGroupVisualData Item;

        [ContextMenu("Test")]
        public void Test()
        {
            SetItem(Item);
        }
#endif
    }
}
