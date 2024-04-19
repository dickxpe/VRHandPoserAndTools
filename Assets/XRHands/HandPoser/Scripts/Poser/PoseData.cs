// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedric

using UnityEngine;

namespace InteractionsToolkit.Poser
{
    public class PoseData : ScriptableObject
    {
        public HandPoseJoints LeftJoints;
        public HandPoseJoints RightJoints;

        public PoseTransform LeftParentTransform;
        public PoseTransform RightParentTransform;

        public void SaveLeftHandData(HandPoseJoints leftData, Transform leftParent)
        {
            LeftJoints = leftData;
            LeftParentTransform = new PoseTransform
            {
                LocalPosition = leftParent.localPosition,//new Vector3(leftParent.localPosition.x / leftParent.localScale.x, leftParent.localPosition.y / leftParent.localScale.y, leftParent.localPosition.z / leftParent.localScale.z),
                LocalRotation = leftParent.localRotation
            };

        }

        public void SaveRightHandData(HandPoseJoints rightData, Transform rightParent)
        {
            RightJoints = rightData;
            RightParentTransform = new PoseTransform
            {
                LocalPosition = rightParent.localPosition, //new Vector3(rightParent.localPosition.x / rightParent.localScale.x, rightParent.localPosition.y / rightParent.localScale.y, rightParent.localPosition.z / rightParent.localScale.z),
                LocalRotation = rightParent.localRotation
            };
        }
    }
}