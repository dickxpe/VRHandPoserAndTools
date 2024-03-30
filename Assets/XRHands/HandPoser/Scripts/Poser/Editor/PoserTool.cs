using System;
using InteractionsToolkit.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

namespace InteractionsToolkit.Poser
{
    public class PoserTool : MonoBehaviour
    {
        [Space]
        [SerializeField] private PoseData poseData;

        public PoseData PoseData
        {
            get => poseData;
            set => poseData = value;
        }
        private PoserManager manager;

        public GameObject leftHandParent;
        public GameObject rightHandParent;

        public PoserHand leftHand;
        public PoserHand rightHand;

        public GameObject poseGameObject;

        public float GetHandDistance()
        {
            if (leftHand && rightHand)
            {
                return Vector3.Distance(leftHandParent.transform.localPosition, rightHandParent.transform.localPosition);
            }
            return 0;

        }

        public PoserTool(GameObject gameObject)
        {
            poseGameObject = gameObject;
            if (!manager) manager = FindObjectOfType<PoserManager>();

            HandPose handpose = poseGameObject.GetComponent<HandPose>();
            if (handpose)
            {
                PoseData = handpose.PrimaryPose;
            }
            else
            {
                PoseData = null;
            }
        }

        public void AdjustHandDistance(float distance)
        {
            if (leftHandParent == null || rightHandParent == null) return;

            var halfDistance = distance * 0.5f;

            var leftPosition = leftHandParent.transform.localPosition;
            leftPosition = new Vector3(halfDistance * -1, leftPosition.y, leftPosition.z);
            leftHandParent.transform.localPosition = leftPosition;

            var rightPosition = rightHandParent.transform.localPosition;
            rightPosition = new Vector3(halfDistance, rightPosition.y, rightPosition.z);
            rightHandParent.transform.localPosition = rightPosition;
        }

        public void DefaultPose() => SetEditorPose(manager.DefaultPose);

        public void MirrorLeftToRight()
        {
            if (!leftHand || !rightHand) return;

            MirrorHand(leftHandParent.transform, rightHandParent.transform);
            MirrorJoints(leftHand, rightHand);
        }

        public void MirrorRightToLeft()
        {
            if (!leftHand || !rightHand) return;

            MirrorHand(rightHandParent.transform, leftHandParent.transform);
            MirrorJoints(rightHand, leftHand);
        }

        public bool HandExists()
        {
            if (rightHandParent || leftHandParent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddOrRemoveHands()
        {
            if (HandExists())
            {
                DestroyImmediate(leftHandParent);
                DestroyImmediate(rightHandParent);
                leftHandParent = null;
                rightHandParent = null;
                leftHand = null;
                rightHand = null;
            }
            else
            {
                ToggleLeftHand();
                ToggleRightHand();
            }
        }

        public void SavePose(string filePath)
        {
            var so = ScriptableObject.CreateInstance<PoseData>();
            if (leftHand) so.SaveLeftHandData(leftHand.CreatePose(), leftHandParent.transform);
            if (rightHand) so.SaveRightHandData(rightHand.CreatePose(), rightHandParent.transform);

            AssetDatabase.CreateAsset(so, filePath);
        }

        public void ScrubPose(float value)
        {
            if (leftHand) leftHand.SetScrubPose(manager.DefaultPose.LeftJoints, poseData.LeftJoints, value);
            if (rightHand) rightHand.SetScrubPose(manager.DefaultPose.RightJoints, poseData.RightJoints, value);
        }

        public void SelectLeftHandParent()
        {
            if (leftHand && leftHand.gameObject.activeSelf) Selection.SetActiveObjectWithContext(leftHandParent, null);
        }

        public void SelectRightHandParent()
        {
            if (rightHand && rightHand.gameObject.activeSelf) Selection.SetActiveObjectWithContext(rightHandParent, null);
        }

        public void SelectLeftHand()
        {
            if (leftHand && leftHand.gameObject.activeSelf) Selection.SetActiveObjectWithContext(leftHand, null);
        }

        public void SelectRightHand()
        {
            if (rightHand && rightHand.gameObject.activeSelf) Selection.SetActiveObjectWithContext(rightHand, null);
        }

        public void ShowPose() => SetEditorPose(poseData);

        public void ToggleLeftHand()
        {
            if (CheckParentGameObjectExists(ref leftHandParent, nameof(leftHandParent), ref leftHand, manager.LeftPrefab))
            {
                leftHandParent.SetActive(!leftHandParent.activeSelf);

            }
            ShowPose();
        }

        public void ToggleRightHand()
        {
            if (CheckParentGameObjectExists(ref rightHandParent, nameof(rightHandParent), ref rightHand, manager.RightPrefab))
            {
                rightHandParent.SetActive(!rightHandParent.activeSelf);

            }
            ShowPose();
        }

        private bool CheckParentGameObjectExists(ref GameObject parentGO, string goName, ref PoserHand poserHand, PoserHand prefab)
        {
            if (!parentGO)
            {
                parentGO = new GameObject(goName);
                parentGO.transform.localPosition = Vector3.zero;
                parentGO.transform.localRotation = Quaternion.identity;
                parentGO.transform.SetParent(poseGameObject.transform);
                parentGO.name += " ---Rotate & Position Me---";

                poserHand = Instantiate(prefab, parentGO.transform);
                var poserHandTransform = poserHand.transform;
                poserHandTransform.localPosition = Vector3.zero;
                poserHandTransform.localRotation = Quaternion.identity;
                poserHand.gameObject.SetActive(true);
                if (poserHand.Type == Handedness.Left)
                {
                    poserHand.name = "Left Hand ---DO NOT Rotate or Position Me // Rotate joints only---";
                }
                else
                {
                    poserHand.name = "Right Hand ---DO NOT Rotate or Position Me // Rotate joints only---";
                }


                parentGO.AddComponent<PoserHandParent>();

                if (poserHand == leftHand)
                {
                    leftHandParent.transform.position = poseGameObject.transform.position + Vector3.left * 0.1f;
                    leftHandParent = parentGO;
                }
                else
                {
                    rightHandParent.transform.position = poseGameObject.transform.position + Vector3.right * 0.1f;
                    rightHandParent = parentGO;
                }

                return false;
            }

            return true;
        }

        public void SetLeftHandAndParent(PoserHand hand, GameObject handParent)
        {
            leftHandParent = handParent;
            leftHand = hand;

        }

        public void SetRightHandParent(PoserHand hand, GameObject handParent)
        {
            rightHandParent = handParent;
            rightHand = hand;
        }

        private void SetEditorPose(PoseData data)
        {
            if (leftHand && data && data.LeftJoints.GetTotalJointCount() != 0)
            {
                leftHand.SetPose(data.LeftJoints);
                leftHandParent.transform.localPosition = data.LeftParentTransform.LocalPosition;
                leftHandParent.transform.localRotation = data.LeftParentTransform.LocalRotation;
            }

            if (rightHand && data && data.RightJoints.GetTotalJointCount() != 0)
            {
                rightHand.SetPose(data.RightJoints);
                rightHandParent.transform.localPosition = data.RightParentTransform.LocalPosition;
                rightHandParent.transform.localRotation = data.RightParentTransform.LocalRotation;
            }
        }

        private static void MirrorJoints(PoserHand source, PoserHand copy)
        {
            for (var i = 0; i < source.HandJoints.jointGroups.Count; i++)
            {
                for (var j = 0; j < source.HandJoints.jointGroups[i].joints.Count; j++)
                {
                    var rotation = source.HandJoints.jointGroups[i].joints[j].localRotation;
                    copy.HandJoints.jointGroups[i].joints[j].localRotation = rotation;
                }
            }
        }

        private static void MirrorHand(Transform source, Transform copy)
        {
            var localPosition = source.localPosition;
            var newPosition = new Vector3(copy.localPosition.x, localPosition.y, localPosition.z);
            copy.localPosition = newPosition;

            var localRotation = source.localRotation;

            // https://forum.unity.com/threads/how-to-mirror-a-euler-angle-or-rotation.90650/
            copy.localRotation = new Quaternion(localRotation.x * -1.0f, localRotation.y, localRotation.z,
                                                localRotation.w * -1.0f);
        }
    }
}