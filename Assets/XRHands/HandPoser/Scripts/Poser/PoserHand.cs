// Author: Cody Tedrick https://github.com/ctedrick
// Edited by: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Cody Tedrick - Copyright (c) 2024 Peter Dickx

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InteractionsToolkit.Poser
{
    public class PoserHand : MonoBehaviour
    {

#if UNITY_EDITOR
        public bool isEditing;
#endif


        [Header("Hand Properties")]

        public Handedness Type;
        public Transform AttachTransform;

        public GameObject ghostHandGameObject;

        [HideInInspector]
        public PoserHand ghostHand;

        private List<Coroutine> PoseCoroutines = new List<Coroutine>();
        private Dictionary<int, List<Coroutine>> MaskedPoseCoroutines = new Dictionary<int, List<Coroutine>>();

        [SerializeField] private HandJoints handJoints;
        public HandJoints HandJoints => handJoints;
        private bool isPosing = false;

        private AnimateHandOnInput animateHandOnInput;

        public void SetIsPosing(bool isPosing)
        {
            this.isPosing = isPosing;
        }

        public void ResetHandPose()
        {
            animateHandOnInput.OpenCompleteHand();
        }

        private void Start()
        {
            animateHandOnInput = GetComponent<AnimateHandOnInput>();
        }

        private void Awake()
        {

            if (ghostHandGameObject != null)
            {
                ghostHand = ghostHandGameObject.GetComponent<PoserHand>();
            }
            if (handJoints == null || handJoints.GetTotalJointCount() == 0)
            {
                Debug.LogError($"{nameof(handJoints)} is null or empty. Check {name}");
                enabled = false;
            }
        }

        public HandPoseJoints CreatePose()
        {
            var handPoseData = new HandPoseJoints();
            for (var i = 0; i < handJoints.jointGroups.Count; i++)
            {
                var handPoseJointGroup = new HandPoseJointGroup();
                handPoseData.poseJointGroups.Add(handPoseJointGroup);

                for (var j = 0; j < handJoints.jointGroups[i].joints.Count; j++)
                {
                    var handPoseJoint = new PoseTransform
                    {
                        LocalRotation = handJoints.jointGroups[i].joints[j].localRotation

                    };
                    handPoseJointGroup.poseJoints.Add(handPoseJoint);
                }

            }

            return handPoseData;
        }

        public void SetScrubPose(HandPoseJoints poseFrom, HandPoseJoints poseTo, float value)
        {
            if (poseFrom == null || poseFrom.GetTotalJointCount() == 0) return;
            if (poseTo == null || poseTo.GetTotalJointCount() == 0) return;

            value = Mathf.Clamp01(value);

            for (var i = 0; i < handJoints.jointGroups.Count; i++)
            {
                for (var j = 0; j < handJoints.jointGroups[i].joints.Count; j++)
                {

                    handJoints.jointGroups[i].joints[j].localRotation = GetScrubRotation(poseFrom.poseJointGroups[i].poseJoints[j].LocalRotation, poseTo.poseJointGroups[i].poseJoints[j].LocalRotation, value);
                }
            }
        }

        public void SetMaskedPose(PoseData poseData, bool[] mask, float inputValue, float duration = 0.1f)
        {
            SetMaskedPose(Type == Handedness.Left ? poseData.LeftJoints : poseData.RightJoints, mask, inputValue, duration);
        }

        public void SetMaskedPose(HandPoseJoints pose, bool[] mask, float inputValue, float duration = 0.1f)
        {
            if (pose == null || pose.GetTotalJointCount() == 0 || isPosing) return;

            foreach (Coroutine coroutine in PoseCoroutines)
            {
                StopCoroutine(coroutine);
            }

            for (var i = 0; i < handJoints.jointGroups.Count; i++)
            {
                if (mask[i])
                {
                    if (MaskedPoseCoroutines.ContainsKey(i))
                    {
                        foreach (Coroutine coroutine in MaskedPoseCoroutines[i])
                        {
                            StopCoroutine(coroutine);
                        }
                        MaskedPoseCoroutines.Remove(i);
                    }
                    List<Coroutine> maskCoroutines = new List<Coroutine>();
                    MaskedPoseCoroutines.Add(i, maskCoroutines);
                    for (var j = 0; j < handJoints.jointGroups[i].joints.Count; j++)
                    {
                        if (Application.isPlaying)
                        {
                            maskCoroutines.Add(StartCoroutine(RotationRoutine(handJoints.jointGroups[i].joints[j].transform, pose.poseJointGroups[i].poseJoints[j].LocalRotation, duration, inputValue)));
                        }
                        else
                        {
                            handJoints.jointGroups[i].joints[j].localRotation = pose.poseJointGroups[i].poseJoints[j].LocalRotation;
                        }

                    }
                }
            }
        }

        public void SetPose(PoseData poseData, float duration = 0.2f, Action callback = null)
        {
            SetPose(Type == Handedness.Left ? poseData.LeftJoints : poseData.RightJoints, duration, callback);
        }

        public void SetPose(HandPoseJoints pose, float duration = 0.2f, Action callback = null)
        {

            if (pose == null || pose.GetTotalJointCount() == 0) return;

            foreach (Coroutine coroutine in PoseCoroutines)
            {
                StopCoroutine(coroutine);
            }

            for (var i = 0; i < handJoints.jointGroups.Count; i++)
            {
                for (var j = 0; j < handJoints.jointGroups[i].joints.Count; j++)
                    if (Application.isPlaying)
                    {
                        PoseCoroutines.Add(StartCoroutine(RotationRoutine(handJoints.jointGroups[i].joints[j].transform, pose.poseJointGroups[i].poseJoints[j].LocalRotation, duration, 1, callback)));
                    }
                    else
                    {
                        handJoints.jointGroups[i].joints[j].localRotation = pose.poseJointGroups[i].poseJoints[j].LocalRotation;
                    }
            }
        }

        private static Quaternion GetScrubRotation(Quaternion from, Quaternion end, float value) => Quaternion.Lerp(from, end, value);

        private static IEnumerator RotationRoutine(Transform target, Quaternion end, float duration, float inputValue = 1, Action callback = null)
        {
            var time = 0f;
            var start = target.localRotation;

            Quaternion percentage = Quaternion.Lerp(start, end, inputValue);
            while (time < duration)
            {

                target.localRotation = Quaternion.Lerp(start, percentage, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            target.localRotation = percentage;

            if (callback != null)
            {
                callback.Invoke();
            }

        }
    }
}