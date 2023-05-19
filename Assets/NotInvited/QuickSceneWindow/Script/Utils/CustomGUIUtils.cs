#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace NotInvited.QuickSceneWindow.Utils
{
    public static class CustomGUIUtils
    {
        public static Color BlueColor = new Color(0.156f, 0.54f, 0.76f);

        public static Color RedColor = new Color(0.8f, 0.1f, 0.2f);

        public static Color YellowColor = new Color(0.9921f, 0.7496f, 0.2313f);

        public static Color DarkGrey = new Color(0.5f, 0.5f, 0.5f);


        public static GUIStyle FoldoutTextWhite
        {
            get
            {
                GUIStyle style = new GUIStyle(EditorStyles.foldout);
                style.fontStyle = FontStyle.Bold;
                SetTextColor(style, Color.white);
                return style;
            }
        }

        public static GUIStyle ButtonRed
        {
            get
            {
                GUIStyle style = new GUIStyle("button");

                //SetBackgroundColor(style, RedColor);
                return style;
            }
        }

        public static GUIStyle RedBackground
        {
            get
            {
                GUIStyle style = new GUIStyle();
                SetBackgroundColor(style, new Color(1f, 0f, 0f));
                return style;
            }
        }

        public static GUIStyle BlueBackground
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.normal.background = Texture2DUtils.GetColorTexture(BlueColor);
                SetTextColor(style, Color.white);
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }

        public static GUIStyle GreyDarkBackground
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.normal.background = Texture2DUtils.GetColorTexture(DarkGrey);
                SetTextColor(style, Color.white);
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }

        public static GUIStyle GetSpecificBackgroundColor(Color color, Color? textColor = null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.background = Texture2DUtils.GetColorTexture(color);
            if (textColor != null)
                SetTextColor(style, textColor.Value);
            return style;
        }

        public static void SetBackgroundColor(GUIStyle style, Color color)
        {
            style.normal.background = Texture2DUtils.GetColorTexture(color);
        }

        public static void SetTextColor(GUIStyle style, Color color)
        {
            style.normal.textColor = color;
            style.normal.textColor = color;
            style.onNormal.textColor = color;
            style.hover.textColor = color;
            style.onHover.textColor = color;
            style.focused.textColor = color;
            style.onFocused.textColor = color;
            style.active.textColor = color;
            style.onActive.textColor = color;
        }

        public static bool CloseButton(params GUILayoutOption[] options)
        {
            var icon = EditorGUIUtility.IconContent("d_winbtn_win_close");
            return ColorButton(icon, RedColor,false, options);
        }

        public static bool DeleteButton(params GUILayoutOption[] options)
        {
            var icon = EditorGUIUtility.IconContent("d_TreeEditor.Trash");
            return ColorButton(icon, RedColor, false, options);
        }

        /// <summary>
        /// Get the icon
        /// List of all icons : https://github.com/halak/unity-editor-icons
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static GUIContent GetIcon(string iconName)
        {
            return EditorGUIUtility.IconContent(iconName);
        }

        /// <summary>
        /// Draw a color button
        /// </summary>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool ColorButton(GUIContent label,Color color, bool pressed = false, params GUILayoutOption[] options)
        {
            var standardColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            bool result = Button(label, pressed, options);
            GUI.backgroundColor = standardColor;
            return result;
        }

        public static bool Button(GUIContent label, bool pressed = false, params GUILayoutOption[] options)
        {
            GUIStyle style = new GUIStyle("button");
            if (pressed)
                style.normal = style.active;
            return GUILayout.Button(label, style, options);
        }

        public static bool Button(string label, bool pressed = false, params GUILayoutOption[] options)
        {
            return Button(new GUIContent(label), pressed, options);
        }

        /// <summary>
        /// Draw a color button
        /// </summary>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool ColorButton(string label, Color color, bool pressed = false, params GUILayoutOption[] options)
        {
            GUIContent guiContentLabel = new GUIContent(label);
            return ColorButton(guiContentLabel, color, pressed, options);
        }

        public static bool DeleteButtonWithCondirmation(string title, string content, params GUILayoutOption[] options)
        {
            return DeleteButton(options) && EditorUtility.DisplayDialog(
                title,
                content, "Delete", "Cancel");
        }
    }
}
#endif