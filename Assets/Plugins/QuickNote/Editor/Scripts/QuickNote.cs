using System;

namespace QuickNoteEditor
{
    [Serializable]
    internal class QuickNote
    {
        public string note;
        internal QColor BackgroundQColor;
        internal QColor TextQColor;
        internal QuickNoteSettings setting;
    }
}