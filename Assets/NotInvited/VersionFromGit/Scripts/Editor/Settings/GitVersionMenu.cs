using NotInvited.VersionFromGit.Editor.Git;
using UnityEditor;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor
{
    public static class GitVersionMenu
    {

        private const string AssetStoreUrl = "https://assetstore.unity.com/packages/slug/203089#reviews";

        private const int menuOrder = 300;

        [MenuItem("Tools/Version From Git/🏷️ Set version manually", priority = menuOrder)]
        public static void SetVersionManually()
        {
            string version = GitData.GetCurrentGitData().GetFormattedVersion();
            PlayerSettings.bundleVersion = version;
        }

        [MenuItem("Tools/Version From Git/💖 Rate the asset!", priority = menuOrder + 1)]
        public static void RateUs()
        {
            Application.OpenURL(AssetStoreUrl);
        }

        [MenuItem("Tools/Version From Git/⚙ Settings", priority = menuOrder + 2)]
        public static void ShowGitVersionSettingsPanel()
        {
            SettingsService.OpenProjectSettings("Project/VersionFromGit");
        }
    }
}