#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NotInvited.QuickSceneWindow
{
    public static class QuickSceneMenu
    {

        private const string AssetStoreUrl = "https://assetstore.unity.com/packages/slug/207596#reviews";

        private const int menuOrder = 300;


        [MenuItem("Tools/Quick Scene Window/💖 Rate the asset!", priority = menuOrder + 1)]
        public static void RateUs()
        {
            Application.OpenURL(AssetStoreUrl);
        }
    }
}
#endif