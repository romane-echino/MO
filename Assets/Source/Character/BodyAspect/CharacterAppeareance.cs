using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MO.Character.BodyAspect
{
    public class CharacterAppeareance : MonoBehaviour
    {
        public Texture2D Texture;

        public CharacterBodyPartData BodyPartData;
        public CharacterColors Colors;

        [SerializeField, Header("References")]
        private List<BodyPartRenderer> bodyPartRenderers = new List<BodyPartRenderer>();

        private void Awake()
        {
            ApplyColors();
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
}

