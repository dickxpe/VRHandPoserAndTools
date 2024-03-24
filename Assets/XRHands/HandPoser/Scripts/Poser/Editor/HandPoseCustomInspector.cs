using InteractionsToolkit.Core;
using InteractionsToolkit.Poser;
using UnityEditor;
using UnityEngine;

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
