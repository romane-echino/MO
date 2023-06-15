using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MO.Item;
using System.Linq;

namespace MO.Character.BodyAspect
{
    public class CharacterAppeareance : MonoBehaviour
    {
        public Texture2D Texture;

        public CharacterBodyPartData BodyPartData;
        public CharacterColors Colors;

        public List<BodyPartType> HiddenBodyParts = new List<BodyPartType>();

        public ItemObject EquipedWeapon { get; private set; } = null;

        [SerializeField, Header("References")]
        private List<BodyPartRenderer> bodyPartRenderers = new List<BodyPartRenderer>();

        [SerializeField]
        private List<BodyPartAnchor> bodyPartAnchors = new List<BodyPartAnchor>();

        private Dictionary<string, GameObject> equipedItems = new Dictionary<string, GameObject>();


        private void Awake()
        {
            ApplyColors();
            HideBodyPart();
        }

        public void ApplyEquipedItems(ItemObject[] items)
        {
            foreach (var key in equipedItems.Keys)
            {
                Destroy(equipedItems[key]);
            }
            equipedItems.Clear();
            HideBodyPart();
            EquipedWeapon = null;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if(item == null)
                    continue;
                if (i == (int)ItemType.EquipedWeapon)
                    EquipedWeapon = item;

                var newGameObject = CreateItem(item.Id);
                equipedItems.Add(item.Id, newGameObject);
            }
        }

        public GameObject CreateItem(string id){
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            var itemVisualData = itemManager.GetItemVisualData(id);
            GameObject go = new GameObject($"item_{itemVisualData.Id}");

            BodyPartAnchor anchor = bodyPartAnchors.First(x => x.Type == itemVisualData.AnchorType);

            go.transform.SetParent(anchor.Transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = itemVisualData.Sprite;
            renderer.sortingOrder = itemVisualData.UseSpecificLayerOrder ? itemVisualData.LayerOrder : anchor.LayerOrder;

            // Set special body part visibility
            if(itemVisualData.VisibleBodyParts != null && itemVisualData.VisibleBodyParts.Count > 0){
                foreach (var bodypart in itemVisualData.VisibleBodyParts)
                {
                    bodyPartRenderers.First(x => x.Type == bodypart).Renderer.gameObject.SetActive(true);
                }
            }
            return go;
        }

        private void ApplyColors()
        {
            foreach (var partRenderer in bodyPartRenderers)
            {
                CharacterBodyPartTools.ApplyColorToBodyPart(partRenderer.Type, Colors, partRenderer.Renderer);
            }
        }

        private void HideBodyPart(){
            foreach (var bodypart in HiddenBodyParts)
            {
                if(bodyPartRenderers.Exists(x => x.Type == bodypart))
                    bodyPartRenderers.First(x => x.Type == bodypart).Renderer.gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Apply texture")]
        public void ApplyTexture()
        {
            UnityEditor.Undo.RecordObject(this, "apply texture to character");
            foreach (var partRenderer in bodyPartRenderers)
            {
                UnityEditor.Undo.RecordObject(partRenderer.Renderer, "apply texture to character");
                partRenderer.Renderer.sprite = BodyPartData.GetSprite(partRenderer.Type);
            }
        }
        [ContextMenu("Debug colors")]
        public void DebugColor()
        {
            foreach (var partRenderer in bodyPartRenderers)
            {
                partRenderer.Renderer.color = new Color(Random.value, Random.value, Random.value, 1f);
            }
        }
#endif
    }

    [Serializable]
    public struct BodyPartRenderer
    {
        public string Name => Type.ToString();
        public SpriteRenderer Renderer;
        public BodyPartType Type;
    }

    [Serializable]
    public struct BodyPartAnchor{
        public BodyPartAnchorType Type;
        public Transform Transform;
        public int LayerOrder;
    }

    public enum BodyPartAnchorType{
        None = 0,
        BottomHead = 1,
        TopHead = 2,
        HandL = 3,
        HandR = 4
    }
}

