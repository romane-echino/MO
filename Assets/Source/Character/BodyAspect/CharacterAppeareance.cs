using System;
using System.Collections.Generic;
using UnityEngine;

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
        [ContextMenu("Test")]
        public void ApplyTexture()
        {
            UnityEditor.Undo.RecordObject(this, "apply texture to character");
            foreach (var partRenderer in bodyPartRenderers)
            {
                UnityEditor.Undo.RecordObject(partRenderer.Renderer, "apply texture to character");
                partRenderer.Renderer.sprite = BodyPartData.GetSprite(partRenderer.Type);
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

