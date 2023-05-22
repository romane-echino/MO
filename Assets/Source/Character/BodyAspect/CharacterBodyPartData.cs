using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MO.Character.BodyAspect
{
    public class CharacterBodyPartData : ScriptableObject
    {
        public List<BodyPartType> BodyPartTypes = new List<BodyPartType>();
        public List<Sprite> BodyPartSprites = new List<Sprite>();

        public Sprite GetSprite(BodyPartType type)
        {
            int idx = GetIndexOfBodyPart(type);
            if (idx >= 0)
                return BodyPartSprites[idx];
            
            return null;
        }

        public int GetIndexOfBodyPart(BodyPartType type)
        {
            for (int i = 0; i < BodyPartTypes.Count; i++)
            {
                if (BodyPartTypes[i] == type)
                    return i;
            }
            return -1;
        }

        public void AddPartSprite(BodyPartType bodyPart, Sprite sprite)
        {
            if (BodyPartTypes.Contains(bodyPart))
            {
                int idx = GetIndexOfBodyPart(bodyPart);
                BodyPartSprites[idx] = sprite;
            }
            else
            {
                BodyPartTypes.Add(bodyPart);
                BodyPartSprites.Add(sprite);
            }
        }
    }

    public enum BodyPartType
    {
        Head = 0,
        ArmL = 1,
        ArmR = 2,
        ForearmL = 3,
        ForearmR = 4,
        UpperBody = 5,
        LowerBody = 6,
        LegL = 7,
        LegR = 8,
        Neck = 9,
        NeckShadow = 10,
        LegShadow = 11,
        EyeL = 12,
        EyeR = 13,
        Mouth = 14,
    }
}

