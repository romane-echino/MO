using NotInvited.VersionFromGit.Editor.Git;
using NotInvited.VersionFromGit.Editor.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor.Settings
{
    public static class VersionFromGitSettingsProvider
    {

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/VersionFromGit", SettingsScope.Project)
            {
                label = "Version From Git",

                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = VersionFromGitSettings.GetSerializedSettings();
                    if (settings != null)
                    {
                        EditorGUI.indentLevel++;

                        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
                        headerStyle.fontSize = 18;

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("General", headerStyle);

                        EditorGUILayout.PropertyField(settings.FindProperty("AutomaticOnBuild"), new GUIContent("Automatic On Build"));
                        EditorGUILayout.PropertyField(settings.FindProperty("AllowLogOnEditor"), new GUIContent("Log enable"));


                        DrawVersionPropertyField(settings.FindProperty("DefaultVersion"));

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Format", headerStyle);

                        DrawFormat(settings);

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Git", headerStyle);


                        CheckIfGitIsInstalled(settings);

                        settings.ApplyModifiedProperties();

                        EditorGUI.indentLevel--;
                    }
                },

                keywords = new HashSet<string>(new[] { "Git", "Version", "Automatic", "Build" })
            };

            return provider;
        }

        private static void DrawFormat(SerializedObject settings)
        {
            SerializedProperty versionFormat = settings.FindProperty("VersionFormat");
            SerializedProperty dateFormat = settings.FindProperty("DateFormat");

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                $"Format the version string with available data{Environment.NewLine}" +
                $"{{0}} : Version{Environment.NewLine}" +
                $"{{1}} : Major{Environment.NewLine}" +
                $"{{2}} : Minor{Environment.NewLine}" +
                $"{{3}} : Revision{Environment.NewLine}" +
                $"{{4}} : Commit Hash{Environment.NewLine}" +
                $"{{5}} : Commit Date{Environment.NewLine}" +
                $"{{6}} : Branch{Environment.NewLine}" +
                $"{{7}} : Nb commit since Tag{Environment.NewLine}" +
                $"{{8}} : Full Git tag result"
                , MessageType.Info
                );

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(versionFormat, new GUIContent("Version format"));

            if (versionFormat.stringValue != VersionFromGitSettings.DefaultVersionFormat)
            {
                if (GUILayout.Button("Reset", GUILayout.MaxWidth(70f)))
                {
                    GUI.FocusControl(null);
                    versionFormat.stringValue = VersionFromGitSettings.DefaultVersionFormat;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (GitData.IsFormattedVersionValid(versionFormat.stringValue, dateFormat.stringValue))
            {
                GUI.enabled = false;
                EditorGUILayout.TextField($"Example", GitData.GetExample().GetFormattedVersion(versionFormat.stringValue, dateFormat.stringValue));
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.HelpBox("Format not valid", MessageType.Error);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(dateFormat, new GUIContent("Date format"));

            if (dateFormat.stringValue != VersionFromGitSettings.DefaultDateFormat)
            {
                if (GUILayout.Button("Reset", GUILayout.MaxWidth(70f)))
                {
                    GUI.FocusControl(null);
                    dateFormat.stringValue = VersionFromGitSettings.DefaultDateFormat;
                }
            }

            EditorGUILayout.EndHorizontal();

            GUI.enabled = false;
            EditorGUILayout.TextField($"Example", DateTime.Now.ToString(dateFormat.stringValue));
            GUI.enabled = true;
        }

        private static void DrawVersionPropertyField(SerializedProperty serializedProperty)
        {
            string initialValue = serializedProperty.stringValue;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(serializedProperty, new GUIContent("Default Version"));
            EditorGUILayout.HelpBox("The Default Version is used when no version is founded.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                string newVersion = serializedProperty.stringValue;

                try
                {
                    serializedProperty.stringValue = new Version(newVersion).ToString();
                }
                catch
                {
                    serializedProperty.stringValue = initialValue;
                }
            }
        }

        private static GitData gitFullData;

        private static void CheckIfGitIsInstalled(SerializedObject settings)
        {
            var style = new GUIStyle(EditorStyles.helpBox);

            GUILayout.BeginVertical(style);

            bool isInstalled = settings.FindProperty("GitInstalled").boolValue;
            bool isAvailability = settings.FindProperty("GitAvailable").boolValue;

            GUIContent valid = EditorGUIUtility.IconContent("TestPassed");
            GUIContent failed = EditorGUIUtility.IconContent("TestFailed");
            GUIContent unknow = EditorGUIUtility.IconContent("TestNormal");

            EditorGUILayout.LabelField(new GUIContent(" Git installed", isInstalled ? valid.image : failed.image));
            EditorGUILayout.LabelField(new GUIContent(" Git on this project", isAvailability ? valid.image : failed.image));

            if (gitFullData != null)
            {
                EditorGUILayout.LabelField(new GUIContent(" Tag found", gitFullData.IsVersionTagFound ? valid.image : failed.image));

                EditorGUILayout.HelpBox(gitFullData.ToString(), MessageType.Info);
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent(" Tag found (Press \"Check Git last tag\" button)", unknow.image));
            }

            EditorGUILayout.Space();

            bool isTested = settings.FindProperty("GitAvailableTested").boolValue;

            if (GUILayout.Button("Check Git last tag") || isTested == false)
            {
                gitFullData = null;

                bool gitInstalled = Git.GitUtils.IsGitInstalled();
                bool gitAvailable = false;

                if (gitInstalled)
                {
                    gitAvailable = Git.GitUtils.IsGitAvailableForFolder();
                }

                settings.FindProperty("GitAvailableTested").boolValue = true;

                settings.FindProperty("GitInstalled").boolValue = gitInstalled;
                settings.FindProperty("GitAvailable").boolValue = gitAvailable;

                if (gitAvailable)
                {
                    CustomLog.Log("Git is installed and available for this project");

                    gitFullData = GitData.GetCurrentGitData();
                }
                else if (gitInstalled)
                {
                    CustomLog.Log("Git is installed but not initilized for this project");
                }
                else
                {
                    CustomLog.Log("Git is NOT installed or found. Check environment variable and git installation");
                }
            }

            GUILayout.EndVertical();
        }
    }
}