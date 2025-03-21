﻿// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedrick

using InteractionsToolkit.Utility;
using UnityEditor;
using UnityEngine;

namespace InteractionsToolkit.Poser
{
    public class PoserManager : Singleton<PoserManager>
    {
        [Header("Pose Asset")]
        public PoseData DefaultOpenPose;
        public PoseData DefaultClosedPose;

        [HideInInspector]
        public PoserHand LeftPoserHand;
        [HideInInspector]
        public PoserHand RightPoserHand;

        [Header("Asset Hands")]
        public GameObject LeftPrefab;
        public GameObject RightPrefab;

        protected override void Awake()
        {
            base.Awake();

            if (!LeftPoserHand)
            {
                Transform parent = transform.Find("XR Origin (XR Rig)/Camera Offset/Left Controller");
                LeftPoserHand = ((GameObject)Instantiate(LeftPrefab, parent)).GetComponent<PoserHand>();
                LeftPoserHand.gameObject.name = "Left hand";
#if UNITY_EDITOR
                if (PrefabUtility.GetPrefabInstanceStatus(LeftPoserHand.ghostHandGameObject) == PrefabInstanceStatus.NotAPrefab && PrefabUtility.GetPrefabAssetType(LeftPoserHand.ghostHandGameObject) != PrefabAssetType.NotAPrefab)
                {
                    LeftPoserHand.ghostHandGameObject = Instantiate(LeftPoserHand.ghostHandGameObject, LeftPoserHand.transform.parent);
                    LeftPoserHand.ghostHandGameObject.name = "Left Ghost Hand";
                    LeftPoserHand.ghostHand = LeftPoserHand.ghostHandGameObject.GetComponent<PoserHand>();
                    LeftPoserHand.ghostHandGameObject.SetActive(false);
                }
                else
                {
#endif
                    LeftPoserHand.ghostHand.transform.SetParent(LeftPoserHand.transform.parent);
#if UNITY_EDITOR
                }
#endif
            }

            if (!RightPoserHand)
            {
                Transform parent = transform.Find("XR Origin (XR Rig)/Camera Offset/Right Controller");
                RightPoserHand = ((GameObject)Instantiate(RightPrefab, parent)).GetComponent<PoserHand>();
                RightPoserHand.gameObject.name = "Right hand";
#if UNITY_EDITOR
                if (PrefabUtility.GetPrefabInstanceStatus(RightPoserHand.ghostHandGameObject) == PrefabInstanceStatus.NotAPrefab && PrefabUtility.GetPrefabAssetType(RightPoserHand.ghostHandGameObject) != PrefabAssetType.NotAPrefab)
                {
                    RightPoserHand.ghostHandGameObject = Instantiate(RightPoserHand.ghostHandGameObject, RightPoserHand.transform.parent);
                    RightPoserHand.ghostHandGameObject.name = "Right Ghost Hand";
                    RightPoserHand.ghostHand = RightPoserHand.ghostHandGameObject.GetComponent<PoserHand>();
                    RightPoserHand.ghostHandGameObject.SetActive(false);
                }
                else
                {
#endif
                    RightPoserHand.ghostHand.transform.SetParent(RightPoserHand.transform.parent);
#if UNITY_EDITOR
                }
#endif
            }
        }

        public void ApplyPose(Handedness hand, PoseData pose)
        {
            if (hand == Handedness.Left) LeftPoserHand.SetPose(pose.LeftJoints);
            else RightPoserHand.SetPose(pose.RightJoints);
        }

        public void ApplyPose(PoserHand hand, PoseData pose)
        {
            if (hand.Type == Handedness.Left) LeftPoserHand.SetPose(pose.LeftJoints);
            else RightPoserHand.SetPose(pose.RightJoints);
        }

        public void ApplyDefaultPose(Handedness hand)
        {
            if (hand == Handedness.Left) LeftPoserHand.SetPose(DefaultOpenPose.LeftJoints);
            else RightPoserHand.SetPose(DefaultOpenPose.RightJoints);
        }

        public void ApplyDefaultPose(PoserHand hand)
        {
            if (hand.Type == Handedness.Left) LeftPoserHand.SetPose(DefaultOpenPose.LeftJoints);
            else RightPoserHand.SetPose(DefaultOpenPose.RightJoints);
        }
    }
}