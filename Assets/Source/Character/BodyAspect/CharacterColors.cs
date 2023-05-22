using System;
using UnityEngine;

namespace MO.Character.BodyAspect
{
    [Serializable, CreateAssetMenu(fileName = "CharacterColors", menuName = "Data/Character/Colors")]
    public class CharacterColors : ScriptableObject
    {
        public Color SkinColor;
        public Color DarkSkinColor;
        public Color EyeColor;
        public Color MouthColor;
        public Color TongueColor;

        public Color LowerBodyColor;
        public Color UpperBodyColor;
        public Color FootColor;
        public Color ShadowColor;
    }
}

