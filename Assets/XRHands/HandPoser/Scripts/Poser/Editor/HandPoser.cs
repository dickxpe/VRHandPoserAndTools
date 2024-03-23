using UnityEngine;
using UnityEditor;
using System.Collections;
using InteractionsToolkit.Core;
using InteractionsToolkit.Poser;
using InteractionsToolkit.Utility;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
namespace InteractionsToolkit.Poser
{
    class HandPoser : EditorWindow
    {
        private bool setDistance;
        private float distance;
        private Vector2 minMaxValues;
        private float scrubValue;
        static PoserTool poserTool;
        static GameObject selectedGameObject = null;

        [MenuItem("GameObject/Edit Hand Pose", false, 0)]
        static void EditHandPose(MenuCommand command)
        {
            EditorWindow.GetWindow(typeof(HandPoser));
            selectedGameObject = Selection.gameObjects[Selection.gameObjects.Length - 1];
            poserTool = new PoserTool(selectedGameObject);
            poserTool.PoseData = selectedGameObject.GetComponent<HandPose>().primaryPose;

            PoserHandParent[] handParents = selectedGameObject.GetComponentsInChildren<PoserHandParent>(true);
            bool createLeft = true;
            bool createRight = true;
            foreach (PoserHandParent handParent in handParents)
            {
                PoserHand hand = handParent.gameObject.GetComponentInChildren<PoserHand>(true);
                if (hand)
                {
                    if (hand.Type == Handedness.Left)
                    {
                        createLeft = false;
                        poserTool.SetLeftHandAndParent(hand, handParent.gameObject);
                    }
                    else if (hand.Type == Handedness.Right)
                    {
                        createRight = false;
                        poserTool.SetRightHandParent(hand, handParent.gameObject);
                    }
                }
            }
            if (createLeft)
            {
                poserTool.ToggleLeftHand();
            }
            if (createRight)
            {
                poserTool.ToggleRightHand();
            }
        }

        [MenuItem("GameObject/Create new Hand Pose", false, 0)]
        static void CreateNewHandPose(MenuCommand command)
        {
            EditorWindow.GetWindow(typeof(HandPoser));
            if (Selection.gameObjects.Length != 0)
            {
                selectedGameObject = Selection.gameObjects[Selection.gameObjects.Length - 1];
            }
            else
            {
                GameObject go = new GameObject("HandPose");
                selectedGameObject = go;
            }

            selectedGameObject.AddComponent<HandPose>();
            poserTool = new PoserTool(selectedGameObject);
            poserTool.AddOrRemoveHands();
            SetExpanded(selectedGameObject, true);
            Selection.activeGameObject = selectedGameObject;
        }

        public static void SetExpanded(GameObject go, bool expand)
        {

            object sceneHierarchy = GetHierarchyWindowType().GetProperty("sceneHierarchy").GetValue(GetHierarchyWindow());
            var methodInfo = sceneHierarchy.GetType().GetMethod("ExpandTreeViewItem", BindingFlags.NonPublic | BindingFlags.Instance);

            methodInfo.Invoke(sceneHierarchy, new object[] { go.GetInstanceID(), expand });
        }
        static Type GetHierarchyWindowType()
        {
            return typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        }

        static EditorWindow GetHierarchyWindow()
        {
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            return EditorWindow.focusedWindow;
        }


        [MenuItem("GameObject/Edit Hand Pose", true)]
        static bool Validate()
        {
            if (Selection.gameObjects.Length > 0)
            {
                GameObject selectedGO = Selection.gameObjects[Selection.gameObjects.Length - 1];

                if (selectedGO.GetComponent<HandPose>())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        [MenuItem("GameObject/Create new Hand Pose", true)]
        static bool Validate2()
        {
            if (Selection.gameObjects.Length == 0)
            {
                return true;
            }
            else if (Selection.gameObjects[Selection.gameObjects.Length - 1].GetComponent<HandPose>())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void OnGUI()
        {
            if (selectedGameObject != null)
            {
                Undo.RecordObject(selectedGameObject, "Value changed");
                EditorGUI.BeginChangeCheck();
                selectedGameObject = (GameObject)EditorGUILayout.ObjectField(selectedGameObject, typeof(GameObject), true);
                if (EditorGUI.EndChangeCheck())
                {
                    poserTool.PoseData = selectedGameObject.GetComponent<HandPose>().primaryPose;
                    Debug.Log("Objectfield" + selectedGameObject.name);

                }
                GUILayout.Label("PoseData", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                poserTool.PoseData = (PoseData)EditorGUILayout.ObjectField(poserTool.PoseData, typeof(PoseData), true);
                if (EditorGUI.EndChangeCheck())
                {
                    HandPose handPose = selectedGameObject.GetComponent<HandPose>();
                    if (handPose)
                    {
                        handPose.primaryPose = poserTool.PoseData;
                    }

                }

                EditorGUILayout.Space();

                if (poserTool.HandExists())
                {

                    if (GUILayout.Button("Remove Hand(s)", EditorStyles.miniButton))
                    {
                        ResetScrubValue(1);
                        poserTool.AddOrRemoveHands();
                    }
                }
                else
                {
                    if (GUILayout.Button("Create Hands", EditorStyles.miniButton))
                    {
                        ResetScrubValue(1);
                        poserTool.AddOrRemoveHands();
                    }
                }

                if (poserTool.leftHandParent || poserTool.rightHandParent)
                {

                    DrawPlaybackSection();
                    DrawEditSection();
                    DrawSaveSection();
                }
            }

        }

        private void DrawPlaybackSection()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Playback", EditorStyles.boldLabel);
            DrawUILine(Color.grey, 1, 5);

            if (poserTool.PoseData)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }


            EditorGUI.BeginChangeCheck();
            scrubValue = EditorGUILayout.Slider("Scrub", scrubValue, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                if (poserTool.PoseData)
                    poserTool.ScrubPose(scrubValue);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show Pose", EditorStyles.miniButton) && poserTool.PoseData)
            {
                ResetScrubValue(1);
                poserTool.ShowPose();
            }

            if (GUILayout.Button("Default Pose", EditorStyles.miniButton) && poserTool.PoseData)
            {
                ResetScrubValue(0);
                poserTool.DefaultPose();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawEditSection()
        {
            var bold = new GUIStyle { fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };

            #region "Header" 

            EditorGUILayout.Space();
            GUILayout.Label("Edit", EditorStyles.boldLabel);
            DrawUILine(Color.grey, 1, 5);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.Label("Left Hand", bold);
            GUILayout.FlexibleSpace();

            GUILayout.FlexibleSpace();
            GUILayout.Label("Right Hand", bold);
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            DrawUILine(Color.grey, 1, 5);

            #endregion

            #region Body

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle", EditorStyles.miniButton))
            {
                ResetScrubValue(1);
                poserTool.ToggleLeftHand();
            }

            if (GUILayout.Button("Toggle", EditorStyles.miniButton))
            {
                ResetScrubValue(1);
                poserTool.ToggleRightHand();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (!poserTool.leftHandParent.activeSelf)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("Select Parent", EditorStyles.miniButton))
            {
                poserTool.SelectLeftHandParent();
            }
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Select Parent", EditorStyles.miniButton))
            {
                poserTool.SelectRightHandParent();
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (!poserTool.leftHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Select Hand", EditorStyles.miniButton))
            {
                poserTool.SelectLeftHand();
            }
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Select Hand", EditorStyles.miniButton))
            {
                poserTool.SelectRightHand();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();


            if (!poserTool.leftHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Mirror to >>>>", EditorStyles.miniButton))
            {
                ResetScrubValue(1);
                poserTool.MirrorLeftToRight();
            }
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("<<<< Mirror to", EditorStyles.miniButton))
            {
                ResetScrubValue(1);
                poserTool.MirrorRightToLeft();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            #endregion

            #region Set Distance Subsection

            EditorGUILayout.Space();

            setDistance = EditorGUILayout.Toggle("Set Distance", setDistance);

            if (setDistance)
            {
                minMaxValues = EditorGUILayout.Vector2Field("Min/Max", minMaxValues);
                distance = poserTool.GetHandDistance();
                minMaxValues.Set(0, 1);

                distance = EditorGUILayout.Slider("Horizontal Distance", distance, minMaxValues.x, minMaxValues.y);
                poserTool.AdjustHandDistance(distance);
            }

            #endregion
        }

        private void DrawSaveSection()
        {
            EditorGUILayout.Space();

            DrawUILine(Color.grey, 1, 5);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Save New Pose", EditorStyles.miniButton))
            {
                ResetScrubValue(1);

                var path = EditorUtility.SaveFilePanel("Save as asset", "Assets/", "Pose", "asset");
                if (path.Length != 0)
                {
                    poserTool.SavePose(path.ConvertToProjectRelativePath());

                    poserTool.PoseData = AssetDatabase.LoadAssetAtPath<PoseData>(path.ConvertToProjectRelativePath());
                    selectedGameObject.GetComponent<HandPose>().primaryPose = poserTool.PoseData;
                }
            }
        }

        private void ResetScrubValue(float value)
        {
            if (!poserTool.PoseData) return;

            scrubValue = value;
            poserTool.ScrubPose(value);
        }

        private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding * 0.5f;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}