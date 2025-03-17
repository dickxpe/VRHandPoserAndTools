using UnityEngine;

namespace QuickNoteEditor
{
    [CreateAssetMenu(fileName = "Note", menuName = "Quick Note/Note", order = 0)]
    internal class QuickNoteObject : ScriptableObject
    {
        public QuickNote note;
        public QuickNoteSettings settings;
        public QuickNoteWindowSettings windowSettings;
        public void OnEnable()
        {
            windowSettings ??= new QuickNoteWindowSettings();
        }
        public void OnValidate()
        {
            note.setting = settings;
        }
    }
}