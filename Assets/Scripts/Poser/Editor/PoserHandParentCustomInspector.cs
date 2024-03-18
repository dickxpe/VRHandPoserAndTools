using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InteractionsToolkit.Poser
{
    [CustomEditor(typeof(PoserHandParent))]
    public class PoserHandParentCustomInspector : Editor
    {
        private PoserHandParent handParent;
        private const float Radius = 0.015f;
        private const float ClickRadius = 0.015f;
        private bool isEditing;
        private string editButtonText;


        public void SetIsEditing(bool isEditing)
        {
            this.isEditing = isEditing;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            handParent = target as PoserHandParent;
            if (!handParent) return;

            GUI.color = isEditing ? Color.red : Color.green;
            editButtonText = isEditing ? "Finish Posing Hand" : "Start Posing Hand";

            if (GUILayout.Button(editButtonText, EditorStyles.miniButton))
            {
                if (isEditing)
                {
                    var tool = handParent.transform.GetComponentInParent<PoserTool>();
                    if (tool)
                    {
                        Selection.SetActiveObjectWithContext(tool, null);
                    }
                }
                else
                {
                    Tools.current = Tool.Move;
                    SceneView.RepaintAll();
                }

                ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                isEditing = !isEditing;
            }
        }

        private void OnSceneGUI()
        {
            if (!isEditing) return;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (!handParent) return;
            var lookRotation = Quaternion.LookRotation(Camera.current.transform.forward);

            Selection.SetActiveObjectWithContext(handParent, null);

            Handles.color = new Color(255, 0, 0, 0.25f);

            Handles.DrawSolidDisc(handParent.transform.position, Camera.current.transform.forward, Radius);

            if (Handles.Button(handParent.transform.position, lookRotation, Radius, ClickRadius, Handles.CircleHandleCap))
            {
                Selection.SetActiveObjectWithContext(handParent.transform.transform, null);
            }


        }
    }
}