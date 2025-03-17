using System;
using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    public class QuickNoteMenu : EditorWindow
    {
        private const string KSearchParams = "t:" + nameof(QuickNoteObject);
        private QuickNoteObject _selectedNote;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/Quick Note/Menu #%Q")]
        public static void ShowWindow()
        {
            GetWindow<QuickNoteMenu>("Quick Notes");
        }

        public void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            if (_selectedNote)
            {
                DrawSelectedNote();
            }
            else
            {
                Header();
                SelectionMenu();
            }

            EditorGUILayout.EndScrollView();
        }

        private void Header()
        {
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                GUILayout.Label("Quick Notes", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("New Note", GUILayout.Width(100), GUILayout.Height(25)))
                {
                    CreateNewNote();
                }
            }
        }

        private void CreateNewNote()
        {
            var note = CreateInstance<QuickNoteObject>();
            var noteIndex = GetNextNoteIndex();
            note.name = $"Note{noteIndex}";
            AssetDatabase.CreateAsset(note, $"Assets/Plugins/QuickNote/Editor/Notes/Note{noteIndex}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private int GetNextNoteIndex()
        {
            var listOfNotes = AssetDatabase.FindAssets(KSearchParams);
            int maxIndex = 0;
            foreach (var noteGuid in listOfNotes)
            {
                var notePath = AssetDatabase.GUIDToAssetPath(noteGuid);
                var noteName = System.IO.Path.GetFileNameWithoutExtension(notePath);
                if (noteName.StartsWith("Note") && int.TryParse(noteName.Substring(4), out int index))
                {
                    maxIndex = Mathf.Max(maxIndex, index);
                }
            }
            return maxIndex + 1;
        }

        private void DrawSelectedNote()
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Import"), GUILayout.Width(25), GUILayout.Height(25)))
            {
                SaveNote();
                _selectedNote = null;
                return;
            }

            QuickNoteGUI.NoteField(_selectedNote, ref _scrollPosition, new QuickNoteWindowSettings
            {
                Locked = false,
                WindowRect = position,
            });
        }

        private void OnLostFocus()
        {
            SaveNote();
        }

        private void SaveNote()
        {
            if (!_selectedNote) return;
            EditorUtility.SetDirty(_selectedNote);
            AssetDatabase.SaveAssets();
        }

        private void SelectionMenu()
        {
            var listOfNotes = AssetDatabase.FindAssets(KSearchParams);
            if (listOfNotes.Length == 0)
            {
                GUILayout.Label("No notes found");
                return;
            }
            foreach (var noteGuid in listOfNotes)
            {
                var notePath = AssetDatabase.GUIDToAssetPath(noteGuid);
                var note = AssetDatabase.LoadAssetAtPath<QuickNoteObject>(notePath);
                if (note)
                {
                    QuickNoteGUI.Line(1f, Color.gray);
                    DrawNote(note);
                }
            }
        }

        private void DrawNote(QuickNoteObject note)
        {
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                GUILayout.Label(note.name);
                GUILayout.FlexibleSpace();
                DrawNoteButtons(note);
            }
        }

        private void DrawNoteButtons(QuickNoteObject note)
        {
            var lButtonStyle = GUI.skin.FindStyle("ButtonLeft");
            var rButtonStyle = GUI.skin.FindStyle("ButtonRight");
            var mButtonStyle = GUI.skin.FindStyle("ButtonMid");
            var options = new[] { GUILayout.Width(25), GUILayout.Height(25) };

            if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("d_ScaleTool").image, "Open Windowed"), lButtonStyle, options))
            {
                QuickNoteWindow.Open(note);
            }

            if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("d_editicon.sml").image, "Edit"), mButtonStyle, options))
            {
                _selectedNote = note;
            }

            if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("TreeEditor.Trash").image, "Delete"), rButtonStyle, options))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(note));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
