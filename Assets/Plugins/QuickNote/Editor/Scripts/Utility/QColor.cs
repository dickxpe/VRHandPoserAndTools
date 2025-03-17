using System;
using UnityEngine;

namespace QuickNoteEditor
{
    [Serializable]
    internal struct QColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color ToUnityColor()
        {
            return new Color(r, g, b, a);
        }
    }
}