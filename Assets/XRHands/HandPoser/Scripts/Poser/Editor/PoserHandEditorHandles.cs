// Author: Cody Tedrick https://github.com/ctedrick
// Edited by: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Cody Tedrick - Copyright (c) 2024 Peter Dickx

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InteractionsToolkit.Poser
{
    [CustomEditor(typeof(PoserHand))]
    public class PoserHandEditorHandles : Editor
    {
        private PoserHand poserHand;

        private const float Radius = 0.005f;
        private const float ClickRadius = 0.005f;
        private string editButtonText;
        bool isFirstHandle = false;

        public bool isEditing;

        string[] searchFingers = { "thumb", "index", "middle", "ring", "pinky" };

        public void recursiveFingerSearch(Transform t)
        {
            foreach (Transform child in t)
            {
                Debug.Log(child.name);

                for (int i = 0; i < searchFingers.Length; i++)
                {
                    if (child.gameObject.name.Contains(searchFingers[i]))
                    {
                        if (poserHand.HandJoints.jointGroups[i].joints.Count < 3)
                        {
                            poserHand.HandJoints.jointGroups[i].joints.Add(child);
                        }
                    }
                }
                recursiveFingerSearch(child);
            }

        }


        public override void OnInspectorGUI()
        {
            Color defaultColor = GUI.color;

            poserHand = target as PoserHand;

            base.OnInspectorGUI();

            GUI.color = Color.cyan;

            editButtonText = "Assign joints automatically";

            if (GUILayout.Button(editButtonText, EditorStyles.miniButton))
            {
                poserHand.HandJoints.jointGroups = new List<HandJointGroup>();
                for (int i = 0; i < 5; i++)
                {
                    HandJointGroup handJointGroup = new HandJointGroup();
                    handJointGroup.joints = new List<Transform>();
                    poserHand.HandJoints.jointGroups.Add(handJointGroup);
                }
                recursiveFingerSearch(poserHand.transform);
            }

            GUI.color = defaultColor;

            if (poserHand.gameObject.tag != "Gamehand")
            {
                GUI.color = poserHand.isEditing ? Color.red : Color.green;
                editButtonText = poserHand.isEditing ? "Finish Posing Joints" : "Start Posing Joints";

                if (GUILayout.Button(editButtonText, EditorStyles.miniButton))
                {
                    if (!poserHand.isEditing)
                    {
                        isFirstHandle = true;
                        Tools.current = Tool.Rotate;
                        ActiveEditorTracker.sharedTracker.isLocked = true;
                    }
                    else
                    {
                        Selection.SetActiveObjectWithContext(poserHand, null);
                        ActiveEditorTracker.sharedTracker.isLocked = false;
                    }
                    SceneView.RepaintAll();
                    EditorWindow.GetWindow(typeof(HandPoser)).Repaint();


                    poserHand.isEditing = !poserHand.isEditing;
                }
            }
        }

        private void OnSceneGUI()
        {
            poserHand = target as PoserHand;

            if (!poserHand.isEditing) return;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));



            if (!poserHand) return;
            var lookRotation = Quaternion.LookRotation(Camera.current.transform.forward);
            if (poserHand && poserHand.HandJoints != null && poserHand.HandJoints.GetTotalJointCount() != 0)
            {

                foreach (var jointGroup in poserHand.HandJoints.jointGroups)
                {
                    foreach (var joint in jointGroup.joints)
                    {
                        if (isFirstHandle)
                        {
                            Selection.SetActiveObjectWithContext(joint, null);
                            isFirstHandle = false;
                        }
                        Handles.color = new Color(255, 0, 0, 0.25f);

                        Handles.DrawSolidDisc(joint.position, Camera.current.transform.forward, Radius);

                        if (Handles.Button(joint.position, lookRotation, Radius, ClickRadius, Handles.CircleHandleCap))
                        {
                            Selection.SetActiveObjectWithContext(joint.transform, null);
                        }
                    }
                }
            }
        }
    }
}