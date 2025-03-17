using UnityEditor;
using UnityEditor.Callbacks;

namespace QuickNoteEditor
{
    public static class QuickNoteDoubleClickDetector
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            QuickNoteObject targetNote = EditorUtility.InstanceIDToObject(instanceID) as QuickNoteObject;
            if (targetNote == null) return false;
            QuickNoteWindow.Open(targetNote);
            return true;
        }
    }
}