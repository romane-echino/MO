#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MO.Character.BodyAspect
{
    public static class CharacterBodyPartTools
    {

        [MenuItem("Tool/Test")]
        public static void Test()
        {
            if (Selection.activeObject is Texture2D)
            {

                if (Selection.assetGUIDs.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
                    var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

                    CharacterBodyPartData data = ScriptableObject.CreateInstance<CharacterBodyPartData>();

                    foreach (var bodyPart in (BodyPartType[])Enum.GetValues(typeof(BodyPartType)))
                    {
                        string spriteBodyPartName = GetBodyPartSpriteName(bodyPart);
                        if (string.IsNullOrWhiteSpace(spriteBodyPartName))
                        {
                            Debug.LogError($"{bodyPart} name is not valid : \"{spriteBodyPartName}\"");
                            continue;
                        }

                        foreach (var sprite in sprites)
                        {
                            if(sprite.name == spriteBodyPartName)
                            {
                                data.AddPartSprite(bodyPart, sprite as Sprite);
                                break;
                            }
                        }
                    }

                    string name = AssetDatabase.GenerateUniqueAssetPath("Assets/BodyPartData.asset");
                    AssetDatabase.CreateAsset(data, name);
                    AssetDatabase.SaveAssets();

                }
            }
        }


        [MenuItem("Tool/MO/Character/Create body part data from selection")]
        public static void CreateCharacterBodyPartFromSelection()
        {
            Transform transform = Selection.activeTransform;

            CharacterBodyPartData data = ScriptableObject.CreateInstance<CharacterBodyPartData>();

            foreach(var bodyPart in (BodyPartType[])Enum.GetValues(typeof(BodyPartType)))
            {
                string bodyPartName = GetBodyPartName(bodyPart);
                if (string.IsNullOrWhiteSpace(bodyPartName))
                {
                    Debug.LogError($"{bodyPart} name is not valid : \"{bodyPartName}\"");
                    continue;
                }

                var child = RecursiveFindChild(transform, bodyPartName);
                if (child == null)
                {
                    Debug.LogError($"{bodyPart} with name : {bodyPartName} was not found on hierarchy");
                    continue;
                }

                var spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    Debug.LogError($"{bodyPart} with name : {bodyPartName} has no sprite renderer");
                    continue;
                }

                data.AddPartSprite(bodyPart, spriteRenderer.sprite);
            }

            string name = AssetDatabase.GenerateUniqueAssetPath("Assets/BodyPartData.asset");
            AssetDatabase.CreateAsset(data, name);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tool/MO/Character/Create body part data from selection", validate = true)]
        public static bool CheckCreateCharacterBodyPartFromSelection()
        {
            Transform transform = Selection.activeTransform;
            return transform != null;
        }

        private static string GetBodyPartName(BodyPartType bodyPart)
        {
            switch (bodyPart)
            {
                default:
                    return bodyPart.ToString();
            }
        }

        private static string GetBodyPartSpriteName(BodyPartType bodyPart)
        {
            switch (bodyPart)
            {
                case BodyPartType.Head:
                    return "Head";
                case BodyPartType.ArmL:
                    return "Arm";
                case BodyPartType.ArmR:
                    return "Arm";
                case BodyPartType.ForearmL:
                    return "Forearm";
                case BodyPartType.ForearmR:
                    return "Forearm";
                case BodyPartType.UpperBody:
                    return "UpperBody";
                case BodyPartType.LowerBody:
                    return "LowerBody";
                case BodyPartType.LegL:
                    return "Leg";
                case BodyPartType.LegR:
                    return "Leg";
                default:
                    throw new NotImplementedException();
            }
        }

        private static Transform RecursiveFindChild(Transform parent, string childName)
        {
            Transform child = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                child = parent.GetChild(i);
                if (child.name == childName)
                {
                    break;
                }
                else
                {
                    child = RecursiveFindChild(child, childName);
                    if (child != null)
                    {
                        break;
                    }
                }
            }
            return child;
        }
    }
}
#endif
