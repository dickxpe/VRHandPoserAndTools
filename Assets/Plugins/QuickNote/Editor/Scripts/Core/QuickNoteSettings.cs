using UnityEditor;
using UnityEngine;

namespace QuickNoteEditor
{
    [CreateAssetMenu(fileName = "QuickNoteSettings", menuName = "Quick Note/Settings")]
    internal class QuickNoteSettings : ScriptableObject
    {
        private static QuickNoteSettings _instance;

        public static QuickNoteSettings Default()
        {
            if (_instance) return _instance;
            _instance = CreateInstance<QuickNoteSettings>();
            AssetDatabase.CreateAsset(_instance, "Assets/Plugins/QuickNote/Editor/Resources/DefaultSettings.asset");
            AssetDatabase.SaveAssets();
            return _instance;
        }
        #region Text

        public Font Font;
        public int FontSize = 14;
        public bool SmartColor = true;
        public bool InverseColor;
        public bool WordWrap = true;
        public bool RichText = true;
        public Color TextColor;

        #endregion

        #region Window
        public bool AutoSave = true;
        [Range(0,1)]
        public float TextBackgroundAlpha = 0.32f;
        public Color StartColor;
        public float BorderPadding = 10;
        #endregion

        private void OnEnable()
        {
            if(!_instance)
                _instance = this;
        }
    }
}