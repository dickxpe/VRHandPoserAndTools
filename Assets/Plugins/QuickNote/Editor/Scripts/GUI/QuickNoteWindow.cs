using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    internal class QuickNoteWindow : EditorWindow, IHasCustomMenu
    {
        private QuickNoteObject _noteObject;
        private Vector2 _scrollPosition;
        private bool _controlPanelEnabled;

        public static void Open(QuickNoteObject noteObject)
        {
            QuickNoteWindow window = CreateInstance<QuickNoteWindow>();
            window.Initialize(noteObject);
            noteObject.windowSettings ??= new QuickNoteWindowSettings();
            if (noteObject.windowSettings.Locked)
            {
                window.ShowPopup();
            }
            else
            {
                window.Show();
            }

            window.position = noteObject.windowSettings.WindowRect == default ? new Rect(100, 100, 400, 400) : noteObject.windowSettings.WindowRect;
        }

        private void Initialize(QuickNoteObject noteObject)
        {
            _noteObject = noteObject;
            titleContent = new GUIContent(noteObject.name);
            name = noteObject.name;
        }

        private void OnGUI()
        {
            if (!_noteObject)
            {
                Close();
                return;
            }
            QuickNoteGUI.BackgroundRect(position, _noteObject.windowSettings);
            QuickNoteGUI.ControlPanel(ref _controlPanelEnabled, ref _noteObject.windowSettings, Lock, Unlock);
            QuickNoteGUI.NoteField(_noteObject, ref _scrollPosition, _noteObject.windowSettings);
            titleContent = new GUIContent(_noteObject.name);
            Repaint();
        }

        private void Lock()
        {
            _noteObject.windowSettings.Locked = true;
            _noteObject.windowSettings.WindowRect = position;
            Open(_noteObject);
            Close();
        }

        private void Unlock()
        {
            _noteObject.windowSettings.Locked = false;
            _noteObject.windowSettings.WindowRect = position;
            Open(_noteObject);
            Close();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Settings"), false, () => { Selection.activeObject = _noteObject.settings; });
        }

        private void OnLostFocus()
        {
            if (!_noteObject.settings.AutoSave) return;
            EditorUtility.SetDirty(_noteObject);
            AssetDatabase.SaveAssets();
        }

        private void OnEnable()
        {
            string searchTypeParams = " t:QuickNoteObject";
            string[] guids = AssetDatabase.FindAssets(name + searchTypeParams);
            if (guids.Length > 0)
            {
                _noteObject = AssetDatabase.LoadAssetAtPath<QuickNoteObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }

            Initialize(_noteObject);
        }
    }
    [System.Serializable]
    internal class QuickNoteWindowSettings
    {
        public bool Locked;
        public Rect WindowRect;
        public Color BackgroundColor;
    }
}