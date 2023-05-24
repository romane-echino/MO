using UnityEngine;

namespace MO.Character.BodyAspect
{
    public static class CharacterBodyPartTools
    {
        public static string GetBodyPartName(BodyPartType bodyPart)
        {
            switch (bodyPart)
            {
                default:
                    return bodyPart.ToString();
            }
        }

        public static string GetBodyPartSpriteName(BodyPartType bodyPart)
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
                case BodyPartType.EyeL:
                    return "Eye";
                case BodyPartType.EyeR:
                    return "Eye";
                case BodyPartType.HandL:
                    return "Hand";
                case BodyPartType.HandR:
                    return "Hand";
                default:
                    return bodyPart.ToString();
            }
        }

        public static void ApplyColorToBodyPart(BodyPartType bodyPart, CharacterColors colors, SpriteRenderer renderer)
        {
            var result = GetColorForBodyPart(bodyPart, colors);
            renderer.color = Color.white;
            if (result.colorQty == 0)
                return;
            else if (result.colorQty == 1)
                renderer.material.SetColor("_MainColor", result.mainColor);
            else if(result.colorQty == 2)
            {
                renderer.material.SetColor("_MainColor", result.mainColor);
                renderer.material.SetColor("_SecondaryColor", result.secondaryColor);
            }
        }

        public static (int colorQty, Color mainColor, Color secondaryColor) GetColorForBodyPart(BodyPartType bodyPart, CharacterColors colors)
        {
            switch (bodyPart)
            {
                case BodyPartType.Head:
                    return (2, colors.SkinColor, colors.DarkSkinColor);
                case BodyPartType.ArmL:
                    return (1, colors.SkinColor, Color.magenta);
                case BodyPartType.ArmR:
                    return (1, colors.SkinColor, Color.magenta);
                case BodyPartType.ForearmL:
                    return (1, colors.DarkSkinColor, Color.magenta);
                case BodyPartType.ForearmR:
                    return (1, colors.DarkSkinColor, Color.magenta);
                case BodyPartType.UpperBody:
                    return (1, colors.UpperBodyColor, Color.magenta);
                case BodyPartType.LowerBody:
                    return (1, colors.LowerBodyColor, Color.magenta);
                case BodyPartType.LegL:
                    return (2, colors.SkinColor, colors.FootColor);
                case BodyPartType.LegR:
                    return (2, colors.SkinColor, colors.FootColor);
                case BodyPartType.Neck:
                    return (1, colors.SkinColor, Color.magenta);
                case BodyPartType.NeckShadow:
                    return (1, colors.ShadowColor, Color.magenta);
                case BodyPartType.LegShadow:
                    return (1, colors.ShadowColor, Color.magenta);
                case BodyPartType.EyeL:
                    return (1, colors.EyeColor, Color.magenta);
                case BodyPartType.EyeR:
                    return (1, colors.EyeColor, Color.magenta);
                case BodyPartType.Mouth:
                    return (2, colors.TongueColor, colors.MouthColor);
                case BodyPartType.HandL:
                case BodyPartType.HandR:
                    return (1, colors.SkinColor, Color.magenta);
                default:
                    return (0, Color.magenta, Color.magenta);
            }
        }
    }
}
