using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace InteractionsToolkit.Poser
{
    [CustomEditor(typeof(PoserHand))]
    public class PoserHandEditorHandles : Editor
    {
        private PoserHand poserHand;

        private const float Radius = 0.005f;
        private const float ClickRadius = 0.005f;

        private bool isEditing;
        private string editButtonText;
        bool isFirstHandle = false;

        public void SetIsEditing(bool isEditing)
        {
            this.isEditing = isEditing;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            poserHand = target as PoserHand;

            if (poserHand.gameObject.tag != "Gamehand")
            {
                GUI.color = isEditing ? Color.red : Color.green;
                editButtonText = isEditing ? "Finish Posing Joints" : "Start Posing Joints";

                if (GUILayout.Button(editButtonText, EditorStyles.miniButton))
                {
                    if (isEditing)
                    {
                        var handPose = poserHand.transform.GetComponentInParent<HandPose>();
                        // handPose.PrimaryPose = null;
                        if (handPose) Selection.SetActiveObjectWithContext(handPose, null);
                    }
                    else
                    {
                        isFirstHandle = true;
                        Tools.current = Tool.Rotate;
                        SceneView.RepaintAll();
                    }

                    ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                    isEditing = !isEditing;
                }
            }
        }

        private void OnSceneGUI()
        {
            if (!isEditing) return;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            poserHand = target as PoserHand;

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