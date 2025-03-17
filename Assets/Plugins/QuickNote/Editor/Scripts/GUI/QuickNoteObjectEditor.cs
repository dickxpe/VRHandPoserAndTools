using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    [CustomEditor(typeof(QuickNoteObject))]
    internal class QuickNoteObjectEditor : Editor
    {
        private QuickNoteObject _noteObject;
        private SerializedProperty _noteProperty;
        private SerializedProperty _settingsProperty;

        private void OnEnable()
        {
            _noteObject = (QuickNoteObject)target;
            _noteProperty = serializedObject.FindProperty("note");
            _settingsProperty = serializedObject.FindProperty("settings");
        }

        public override void OnInspectorGUI()
        {
            var icon = EditorGUIUtility.IconContent("d_winbtn_win_max@2x");
            if (GUILayout.Button(new GUIContent(icon), GUILayout.Width(40), GUILayout.Height(40)))
            {
                QuickNoteWindow.Open(_noteObject);
            }
            EditorGUILayout.PropertyField(_settingsProperty, GUILayout.Height(40));
            EditorGUILayout.Space();
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_noteProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}