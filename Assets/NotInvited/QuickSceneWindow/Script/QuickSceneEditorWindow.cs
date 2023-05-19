#if UNITY_EDITOR
using NotInvited.QuickSceneWindow.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NotInvited.QuickSceneWindow
{
    public class QuickSceneEditorWindow : EditorWindow
    {
        private string searchName = "";
        private int searchSelected = -1;

        private int lastCursorIndex = 0;
        private int currentSceneIndex;

        private int newSceneIndexAsked;

        private static bool showOnlyOnBuild = false;

        private List<string> sceneNames = new List<string>();
        private List<string> scenePaths = new List<string>();

        private bool sceneFetch = false;

        private List<string> allScenePaths = new List<string>();
        private List<string> onlyBuildScenePaths = new List<string>();

        private Vector2 scrollViewPosition = Vector2.zero;

        [MenuItem("Window/📋 Quick Scenes Loader", false, 1000)]
        [MenuItem("Tools/Quick Scene Window/📋 Open", priority = 300)]
        public static void ShowWindow()
        {
            showOnlyOnBuild = EditorPrefs.GetBool("QuickSceneWindow_showOnlyOnBuild");
            GetWindow(typeof(QuickSceneEditorWindow), false, "Quick Scene");
        }

        void OnGUI()
        {
            if (sceneFetch == false)
            {
                FetchScenes();
            }

            scenePaths = showOnlyOnBuild ? onlyBuildScenePaths : allScenePaths;
            sceneNames = scenePaths.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            var actualScenePath = EditorSceneManager.GetActiveScene().path.Replace("/", "\\"); ;

            currentSceneIndex = newSceneIndexAsked = scenePaths.FindIndex(x => x == actualScenePath);

            DrawOpenSceneBar();

            scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);
            DrawAllSceneButtons();
            GUILayout.EndScrollView();

            Rect allwindow = new Rect(Vector2.zero, position.size);
            if (GUI.Button(allwindow, "", GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }

        private void FetchScenes()
        {
            EditorUtility.DisplayProgressBar("Searching", $"Searching all scenes on the project", 0f);

            onlyBuildScenePaths = EditorBuildSettings.scenes.OrderBy(x => Path.GetFileNameWithoutExtension(x.path)).Select(x => x.path.Replace("/", "\\")).ToList();
            allScenePaths = GetAllScenePath().OrderBy(x => Path.GetFileNameWithoutExtension(x)).ToList();
            sceneFetch = true;

            EditorUtility.ClearProgressBar();
        }

        private void DrawDropDown()
        {
            newSceneIndexAsked = EditorGUILayout.Popup(currentSceneIndex, sceneNames.ToArray(), GUI.skin.FindStyle("toolbarDropDown"));
            if (currentSceneIndex != newSceneIndexAsked)
            {
                currentSceneIndex = newSceneIndexAsked;
                OpenScene(newSceneIndexAsked);
            }
        }

        private void DrawAllSceneButtons()
        {
            int visibleIndex = 0;
            for (int i = 0; i < sceneNames.Count; i++)
            {
                if (IsSceneButtonShow(sceneNames[i]))
                {
                    EditorGUILayout.BeginHorizontal();

                    GUIContent name = IsSceneInBuild(i) ? new GUIContent(sceneNames[i], EditorGUIUtility.FindTexture("BuildSettings.Standalone.Small")) : new GUIContent($"      {sceneNames[i]}");

                    var style = new GUIStyle(EditorStyles.toolbarButton);
                    style.alignment = TextAnchor.MiddleLeft;
                    bool selected = this.currentSceneIndex == i;
                    var standardColor = GUI.backgroundColor;
                    bool selectStyle = false;
                    if (selected)
                    {
                        GUI.backgroundColor = CustomGUIUtils.BlueColor;
                    }
                    if (visibleIndex == searchSelected)
                    {
                        selectStyle = true;
                    }
                    if (GUILayout.Toggle(selectStyle, name, style) && selected == false && selectStyle == false)
                    {
                        this.currentSceneIndex = newSceneIndexAsked = i;
                        OpenScene(i);
                    }
                    if (visibleIndex == searchSelected && selected == false)
                    {
                        GUILayout.Button("Enter to load", EditorStyles.toolbarButton, GUILayout.MaxWidth(80f));
                    }
                    GUI.backgroundColor = standardColor;
                    EditorGUILayout.EndHorizontal();

                    visibleIndex++;
                }
            }
        }

        private bool IsSceneInBuild(int i)
        {
            var path = scenePaths[i];
            if (onlyBuildScenePaths.Contains(path))
                return true;
            else
                return false;
        }

        private bool IsSceneButtonShow(string sceneName)
        {
            if (string.IsNullOrEmpty(searchName))
                return true;
            else
            {
                if (sceneName.ToLower().Contains(searchName.ToLower()))
                    return true;
                else
                    return false;
            }
        }


        private int SceneButtonShowCount()
        {
            return sceneNames.Count(x => IsSceneButtonShow(x));
        }


        /// <summary>
        /// Display a search bar and handle the search
        /// </summary>
        private void DrawOpenSceneBar()
        {
            //
            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            DrawDropDown();
            GUILayout.FlexibleSpace();
            DrawOnlyOnBuildButton();
            DrawRefreshButton();
            GUILayout.FlexibleSpace();
            GUI.SetNextControlName("searchScene");

            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            var nowCursorIndex = editor.cursorIndex;
            if (nowCursorIndex > lastCursorIndex)
            {
                lastCursorIndex = editor.cursorIndex;
            }

            searchName = GUILayout.TextField(searchName, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MaxWidth(200f));

            if (string.IsNullOrEmpty(searchName))
            {
                searchSelected = -1;
            }
            else if (searchSelected < 0)
            {
                searchSelected = 0;
            }
            else
            {
                searchSelected = Mathf.Clamp(searchSelected, 0, SceneButtonShowCount() - 1);
            }


            if (GUI.GetNameOfFocusedControl() == "searchScene")
            {
                Event evt = Event.current;
                var keyCode = evt.keyCode;
                if (evt.type == EventType.KeyUp)
                {
                    switch (keyCode)
                    {
                        case KeyCode.Return:
                            LoadFirstScene();
                            Repaint();
                            GUIUtility.ExitGUI();
                            break;
                        case KeyCode.Escape:
                            ClearSearchBar(false);
                            Repaint();
                            GUIUtility.ExitGUI();
                            break;
                        case KeyCode.UpArrow:
                            EditorGUIUtility.editingTextField = false;
                            searchSelected = Mathf.Clamp(searchSelected - 1, 0, SceneButtonShowCount() - 1);
                            editor.cursorIndex = editor.selectIndex = editor.text.Length;
                            evt.Use();
                            Repaint();
                            break;
                        case KeyCode.DownArrow:
                            EditorGUIUtility.editingTextField = false;
                            searchSelected = Mathf.Clamp(searchSelected + 1, 0, SceneButtonShowCount() - 1);
                            editor.cursorIndex = editor.selectIndex = editor.text.Length;
                            evt.Use();
                            Repaint();
                            break;
                    }
                }
            }

            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                ClearSearchBar();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawOnlyOnBuildButton()
        {
            GUIContent onlyBuildModeContent = EditorGUIUtility.TrIconContent("BuildSettings.Standalone.Small", "When toggled on, filter only scenes who are in the actual build.");
            var newShowOnlyOnBuild = GUILayout.Toggle(showOnlyOnBuild, onlyBuildModeContent, EditorStyles.toolbarButton);

            if(newShowOnlyOnBuild != showOnlyOnBuild)
            {
                showOnlyOnBuild = newShowOnlyOnBuild;
                EditorPrefs.SetBool("QuickSceneWindow_showOnlyOnBuild", true);
            }
        }

        private void DrawRefreshButton()
        {
            GUIContent refreshContent = EditorGUIUtility.TrIconContent("Refresh", "Get all scenes from the Assets folder");
            if (GUILayout.Button(refreshContent, EditorStyles.toolbarButton))
            {
                sceneFetch = false;
            }
        }

        /// <summary>
        /// Clear the search bar and loose focus if parameter say true
        /// </summary>
        /// <param name="looseFocus"></param>
        private void ClearSearchBar(bool looseFocus = true)
        {
            if (looseFocus)
                GUI.FocusControl(null);
            searchSelected = -1;
            searchName = "";
        }

        private void LoadFirstScene()
        {
            var scenes = sceneNames.Where(x => IsSceneButtonShow(x)).ToList();
            if (scenes.Count == 1)
            {
                ClearSearchBar();
                OpenScene(sceneNames.IndexOf(scenes.FirstOrDefault()));
            }
            else if (scenes.Count > 1)
            {
                var sceneToOpen = scenes[searchSelected];
                int normalListIndex = sceneNames.IndexOf(sceneToOpen);
                if (normalListIndex != currentSceneIndex)
                    OpenScene(normalListIndex);
            }
        }

        /// <summary>
        /// Open a specific scene by giving the _buildScenes index
        /// </summary>
        /// <param name="index"></param>
        private void OpenScene(int index)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePaths[index]);
            GUIUtility.ExitGUI();
        }

        private List<string> GetAllScenePath()
        {
            List<string> scenePaths = new List<string>();
            string folderName = Application.dataPath.Replace("/", "\\");
            var dirInfo = new DirectoryInfo(folderName);
            var allFileInfos = dirInfo.GetFiles("*.unity", SearchOption.AllDirectories);

            foreach (var fileInfo in allFileInfos)
            {
                var testPath = Path.Combine(fileInfo.FullName, "");
                var relativePath = testPath.Replace(folderName, "Assets");
                scenePaths.Add(relativePath);
            }
            return scenePaths;
        }
    }
}
#endif