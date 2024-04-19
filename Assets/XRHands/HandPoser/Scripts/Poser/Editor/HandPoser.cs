// Author: Cody Tedrick https://github.com/ctedrick
// Edited by: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Cody Tedrick - Copyright (c) 2024 Peter Dickx

using UnityEngine;
using UnityEditor;
using System.Collections;
using InteractionsToolkit.Utility;
using System;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
namespace InteractionsToolkit.Poser
{
    class HandPoser : EditorWindow
    {
        private bool setDistance;
        private float distance;
        private Vector2 minMaxValues;
        private static float scrubValue = 1;
        static PoserTool poserTool;
        static GameObject selectedGameObject = null;

        private Color defaultColor;

        [MenuItem("GameObject/Edit Hand Pose", false, 0)]
        static void EditHandPose(MenuCommand command)
        {

            if (FindObjectOfType<PoserManager>())
            {
                EditorWindow.GetWindow(typeof(HandPoser));
                selectedGameObject = Selection.gameObjects[Selection.gameObjects.Length - 1];
                poserTool = new PoserTool(selectedGameObject);
                poserTool.PoseData = selectedGameObject.GetComponent<IHandPose>().PrimaryPose;

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
                ResetScrubValue(1);
                poserTool.ShowPose();
                SceneView.lastActiveSceneView.LookAt(selectedGameObject.transform.position);
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
#pragma warning disable 
            poserTool = new PoserTool(selectedGameObject);
#pragma warning restore
            poserTool.AddOrRemoveHands();
            SetExpanded(selectedGameObject, true);
            Selection.activeGameObject = selectedGameObject;
            SceneView.lastActiveSceneView.LookAt(selectedGameObject.transform.position);
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

                if (selectedGO.GetComponent<IHandPose>() != null)
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
            else if (Selection.gameObjects[Selection.gameObjects.Length - 1].GetComponent<IHandPose>() != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        static IEnumerator IEDelayEditor(HandPoser handPoser)
        {
            while (true)
            {
                handPoser.Repaint();
                yield return new EditorWaitForSeconds(0.25f);
            }
        }


        HandPoser()
        {
            //  EditorCoroutineUtility.StartCoroutine(IEDelayEditor(this), this);
        }

        void OnGUI()
        {
            defaultColor = GUI.color;
            if (selectedGameObject != null)
            {
                if (selectedGameObject.transform.localScale.x != selectedGameObject.transform.localScale.y || selectedGameObject.transform.localScale.x != selectedGameObject.transform.localScale.z)
                {
                    GUILayout.Label("Can't create a pose for a non-uniform scaled object", EditorStyles.boldLabel);
                    GUILayout.Label("Set the scale to a uniform value", EditorStyles.boldLabel);
                    GUILayout.Label("or create an empty parent object with scale 1,1,1", EditorStyles.boldLabel);
                }


                else
                {
                    if (!FindObjectOfType<PoserManager>())
                    {
                        GUILayout.Label("First add a PoserManager to your scene", EditorStyles.boldLabel);
                        EditorGUILayout.Space();

                        if (GUILayout.Button("Create PoserManager", EditorStyles.miniButton))
                        {
                            GameObject poserManager = new GameObject("PoserManager");
                            poserManager.AddComponent<PoserManager>();
                        }
                    }
                    else if (selectedGameObject != null)
                    {
                        Undo.RecordObject(selectedGameObject, "Value changed");
                        EditorGUI.BeginChangeCheck();
                        selectedGameObject = (GameObject)EditorGUILayout.ObjectField(selectedGameObject, typeof(GameObject), true);
                        if (EditorGUI.EndChangeCheck())
                        {
                            poserTool.PoseData = selectedGameObject.GetComponent<IHandPose>().PrimaryPose;
                            Debug.Log("Objectfield" + selectedGameObject.name);

                        }
                        GUILayout.Label("PoseData", EditorStyles.boldLabel);
                        EditorGUI.BeginChangeCheck();
                        poserTool.PoseData = (PoseData)EditorGUILayout.ObjectField(poserTool.PoseData, typeof(PoseData), true);
                        if (EditorGUI.EndChangeCheck())
                        {
                            IHandPose handPose = selectedGameObject.GetComponent<IHandPose>();
                            if (handPose != null)
                            {
                                handPose.PrimaryPose = poserTool.PoseData;
                            }

                        }

                        EditorGUILayout.Space();

                        if (poserTool.HandExists())
                        {
                            GUI.color = Color.red;
                            if (GUILayout.Button("Remove Hand(s)", EditorStyles.miniButton))
                            {
                                ResetScrubValue(1);
                                poserTool.AddOrRemoveHands();
                                DestroyImmediate(selectedGameObject.GetComponent<HandPose>());

                            }
                        }
                        else
                        {
                            GUI.color = Color.green;
                            if (GUILayout.Button("Create Hands", EditorStyles.miniButton))
                            {
                                if (selectedGameObject.GetComponent<IHandPose>() == null)
                                {
                                    selectedGameObject.AddComponent<HandPose>().PrimaryPose = poserTool.PoseData;
                                }
                                ResetScrubValue(1);
                                poserTool.AddOrRemoveHands();
                                SceneView.lastActiveSceneView.LookAt(selectedGameObject.transform.position);
                            }
                        }
                        GUI.color = defaultColor;

                        if (poserTool.leftHandParent || poserTool.rightHandParent)
                        {

                            DrawPlaybackSection();
                            DrawEditSection();
                            DrawSaveSection();
                        }
                    }
                    else
                    {
                        GUILayout.Label("No Pose selected", EditorStyles.boldLabel);
                    }
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
                poserTool.ToggleLeftHand();
            }

            if (GUILayout.Button("Toggle", EditorStyles.miniButton))
            {
                poserTool.ToggleRightHand();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (!poserTool.leftHandParent.activeSelf)
            {
                GUI.enabled = false;
            }

            GUI.color = Color.cyan;
            if (GUILayout.Button("Pose Parent", EditorStyles.miniButton))
            {
                poserTool.SelectLeftHandParent();
                SceneView.lastActiveSceneView.LookAt(poserTool.leftHandParent.transform.position);
            }
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Pose Parent", EditorStyles.miniButton))
            {
                poserTool.SelectRightHandParent();
                SceneView.lastActiveSceneView.LookAt(poserTool.rightHandParent.transform.position);
            }
            GUI.color = defaultColor;
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (!poserTool.leftHandParent.activeSelf)
            {
                GUI.enabled = false;
            }
            GUI.color = poserTool.leftHand.isEditing ? Color.red : Color.green;
            string poseHandText = poserTool.leftHand.isEditing ? "Stop posing" : "Pose hand";
            if (GUILayout.Button(poseHandText, EditorStyles.miniButton))
            {
                if (poserTool.leftHand.isEditing)
                {
                    poserTool.leftHand.isEditing = false;
                    SceneView.RepaintAll();
                    ActiveEditorTracker.sharedTracker.isLocked = false;
                }
                else
                {
                    poserTool.leftHand.isEditing = true;
                    poserTool.rightHand.isEditing = false;
                    ActiveEditorTracker.sharedTracker.isLocked = false;
                    poserTool.SelectLeftHand();
                    SceneView.lastActiveSceneView.LookAt(poserTool.leftHand.transform.position);
                    SceneView.RepaintAll();
                    ActiveEditorTracker.sharedTracker.isLocked = true;

                }


            }
            GUI.color = defaultColor;
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }

            GUI.color = poserTool.rightHand.isEditing ? Color.red : Color.green;
            poseHandText = poserTool.rightHand.isEditing ? "Stop posing" : "Pose hand";
            if (GUILayout.Button(poseHandText, EditorStyles.miniButton))
            {
                if (poserTool.rightHand.isEditing)
                {

                    poserTool.rightHand.isEditing = false;
                    SceneView.RepaintAll();
                    ActiveEditorTracker.sharedTracker.isLocked = false;
                }
                else
                {
                    poserTool.rightHand.isEditing = true;
                    poserTool.leftHand.isEditing = false;
                    ActiveEditorTracker.sharedTracker.isLocked = false;
                    poserTool.SelectRightHand();
                    SceneView.lastActiveSceneView.LookAt(poserTool.rightHand.transform.position);
                    SceneView.RepaintAll();
                    ActiveEditorTracker.sharedTracker.isLocked = true;



                }
            }
            GUI.color = defaultColor;
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

                poserTool.MirrorLeftToRight();
            }
            GUI.enabled = true;

            if (!poserTool.rightHandParent.activeSelf)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("<<<< Mirror to", EditorStyles.miniButton))
            {

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

                var path = EditorUtility.SaveFilePanel("Save as asset", "Assets/", "Pose", "asset");
                if (path.Length != 0)
                {
                    poserTool.SavePose(path.ConvertToProjectRelativePath());

                    poserTool.PoseData = AssetDatabase.LoadAssetAtPath<PoseData>(path.ConvertToProjectRelativePath());
                    selectedGameObject.GetComponent<IHandPose>().PrimaryPose = poserTool.PoseData;
                }
            }
        }

        private static void ResetScrubValue(float value)
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