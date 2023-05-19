using UnityEngine;

namespace NotInvited.QuickSceneWindow.Utils
{
    public static class Texture2DUtils
    {
        public static void SetColor(this Texture2D tex2, Color32 color)
        {
            var fillColorArray = tex2.GetPixels32();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = color;
            }

            tex2.SetPixels32(fillColorArray);
            tex2.Apply();
        }

        public static Texture2D GetColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.SetColor(color);
            return texture;
        }
    }
}