// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedrick

using InteractionsToolkit.Poser;
using UnityEditor;

namespace InteractionsToolkit.Poser
{
    [CustomEditor(typeof(HandPose))]
    public class HandPoseCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myScript = target as HandPose;
            myScript.PrimaryPose = (PoseData)EditorGUILayout.ObjectField("Primary pose", myScript.PrimaryPose, typeof(PoseData), true);

        }
    }
}