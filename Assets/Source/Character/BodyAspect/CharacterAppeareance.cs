using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MO.Item;

namespace MO.Character.BodyAspect
{
    public class CharacterAppeareance : MonoBehaviour
    {
        public Texture2D Texture;

        public CharacterBodyPartData BodyPartData;
        public CharacterColors Colors;

        [SerializeField, Header("References")]
        private List<BodyPartRenderer> bodyPartRenderers = new List<BodyPartRenderer>();

        [SerializeField]
        private List<BodyPartAnchor> bodyPartAnchors = new List<BodyPartAnchor>();

        private Dictionary<string, GameObject> equipedItems = new Dictionary<string, GameObject>();

        private void Awake()
        {
            ApplyColors();
        }

        public void ApplyEquipedItems(ItemObject[] items)
        {
            foreach (var key in equipedItems.Keys)
            {
                Destroy(equipedItems[key]);
            }
            equipedItems.Clear();

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if(item == null)
                    continue;
                CreateItem(item.Id);
            }
        }

        private void CreateItem(string id){
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            var itemVisualData = itemManager.GetItemVisualData(id);
            GameObject go = new GameObject($"item_{itemVisualData.Id}");
            foreach(var anchor in bodyPartAnchors){
                if(anchor.Type == itemVisualData.AnchorType){
                    go.transform.SetParent(anchor.Transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                }
            }

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = itemVisualData.Sprite;
            renderer.sortingOrder = itemVisualData.LayerOrder;

            equipedItems.Add(id, go);
        }

        private void ApplyColors()
        {
            foreach (var partRenderer in bodyPartRenderers)
            {
                CharacterBodyPartTools.ApplyColorToBodyPart(partRenderer.Type, Colors, partRenderer.Renderer);
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
    }
}

