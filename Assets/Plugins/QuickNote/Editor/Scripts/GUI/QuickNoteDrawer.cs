using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    [CustomPropertyDrawer(typeof(QuickNote))]
    internal class QuickNoteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Calculate dynamic height for the note text area
            SerializedProperty noteProperty = property.FindPropertyRelative("note");
            float noteHeight =
                QuickNoteUtility.TextAreaStyle.CalcHeight(
                    new GUIContent(noteProperty.stringValue + EditorGUIUtility.singleLineHeight * 3), position.width);

            Rect noteRect = new Rect(position.x - 10, position.y,
                position.width + 10,
                noteHeight);
            noteProperty.stringValue =
                EditorGUI.TextArea(noteRect, noteProperty.stringValue, QuickNoteUtility.TextAreaStyle);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            // Calculate dynamic height for the note field
            SerializedProperty noteProperty = property.FindPropertyRelative("note");
            float noteHeight = QuickNoteUtility.TextAreaStyle.CalcHeight(new GUIContent(noteProperty.stringValue),
                EditorGUIUtility.currentViewWidth);

            // Total height: title, note, background color, text color fields, and padding
            return noteHeight + lineHeight;
        }
    }
}