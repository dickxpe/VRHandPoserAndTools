using InteractionsToolkit.Core;
using InteractionsToolkit.Poser;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandPose))]
public class HandPoseCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as HandPose;
        myScript.primaryPose = (PoseData)EditorGUILayout.ObjectField("Primary pose", myScript.primaryPose, typeof(PoseData), true);
        if (myScript.GetComponent<MultipleGrabInteractable>())
        {
            myScript.secondaryPose = (PoseData)EditorGUILayout.ObjectField("Secondary pose", myScript.secondaryPose, typeof(PoseData), true);
        }

    }
}
