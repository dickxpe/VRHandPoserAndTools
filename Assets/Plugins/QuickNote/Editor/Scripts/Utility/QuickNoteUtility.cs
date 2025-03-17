using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    internal static class QuickNoteUtility
    {
        public static GUIStyle TextAreaStyle => TextAreaWithSettings(QuickNoteSettings.Default());
        
        [MenuItem("Tools/Quick Note/New Note #%T")]
        internal static void CreateNote()
        {
            const string path = "Assets/Plugins/QuickNote/Editor/Notes/";
            QuickNoteObject note = ScriptableObject.CreateInstance<QuickNoteObject>();
            note.note = new QuickNote();
            note.settings = QuickNoteSettings.Default();
            // if note exists, increment the name
            int i = 0;
            while (AssetDatabase.LoadAssetAtPath<QuickNoteObject>("Assets/Plugins/QuickNote/Editor/Notes/Note " + i + ".asset"))
            {
                i++;
            }
            note.name = "Note " + i;
            AssetDatabase.CreateAsset(note, path + note.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            QuickNoteWindow.Open(note);
        }

        public static GUIStyle TextAreaWithSettings(QuickNoteSettings settings)
        {
            GUIStyle style = new GUIStyle(EditorStyles.textArea)
            {
                font = settings.Font,
                wordWrap = settings.WordWrap,
                richText = settings.RichText,
                fontSize = settings.FontSize,
                normal =
                {
                    textColor = settings.SmartColor
                        ? settings.InverseColor ? Color.white : Color.black
                        : settings.TextColor,
                },
                hover =
                {
                    textColor = settings.SmartColor
                        ? settings.InverseColor ? Color.white : Color.black
                        : settings.TextColor
                },
                active =
                {
                    textColor = settings.SmartColor
                        ? settings.InverseColor ? Color.white : Color.black
                        : settings.TextColor
                },
            };
            return style;
        }
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}