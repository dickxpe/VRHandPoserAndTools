using System;
using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    internal static class QuickNoteGUI
    {
        internal static void BackgroundRect(Rect position, QuickNoteWindowSettings settings)
        {
            var rect = new Rect(0, 0, position.width, position.height);
            EditorGUI.DrawRect(rect, settings.BackgroundColor);
        }

        internal static void ControlPanel(ref bool enabled, ref QuickNoteWindowSettings settings, Action onLock,
            Action onUnlock)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();

            var index = GUILayout.Toolbar(!enabled ? -1 : 0,
                new[] { EditorGUIUtility.IconContent("d__Popup") }, "AppCommand");
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck() && index == 0)
            {
                enabled = !enabled;
            }

            if (!enabled) return;

            settings = NoteToolbar(settings, onLock, onUnlock);
        }
        internal static void Line(float height = 1f, Color color = default)
        {
            if (color == default) color = Color.white;
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), GetTexture(color));
        }

        private static QuickNoteWindowSettings NoteToolbar(QuickNoteWindowSettings settings, Action onLock,
            Action onUnlock)
        {
            EditorGUI.BeginChangeCheck();
            var lockContent = EditorGUIUtility.IconContent(!settings.Locked ? "LockIcon-On" : "LockIcon");
            var colorContent = EditorGUIUtility.IconContent("ColorPicker.CycleColor");
            var index2 = GUILayout.Toolbar(-1,
                new[]
                {
                    lockContent,
                    colorContent
                }, "AppCommand");

            if (!EditorGUI.EndChangeCheck()) return settings;

            switch (index2)
            {
                case 0:
                    if (settings.Locked)
                        onUnlock?.Invoke();
                    else
                        onLock?.Invoke();
                    break;
                case 1:
                    ColorPicker.OpenColorPicker(color =>
                    {
                        settings.BackgroundColor = color;
                    },
                        settings.BackgroundColor.WithAlpha(1));
                    break;
            }

            return settings;
        }

        internal static void NoteField(QuickNoteObject noteObject, ref Vector2 scrollPosition,
            QuickNoteWindowSettings settings)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            var style = QuickNoteUtility.TextAreaWithSettings(noteObject.settings);
            GUI.backgroundColor = settings.BackgroundColor.WithAlpha(noteObject.settings.TextBackgroundAlpha);
            noteObject.note.note = EditorGUILayout.TextArea(noteObject.note.note, style, GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndScrollView();
        }
        internal static Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.alphaIsTransparency = true;
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}