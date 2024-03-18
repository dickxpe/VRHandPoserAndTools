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
                LocalPosition = leftParent.localPosition,
                LocalRotation = leftParent.localRotation
            };
        }

        public void SaveRightHandData(HandPoseJoints rightData, Transform rightParent)
        {
            RightJoints = rightData;
            RightParentTransform = new PoseTransform
            {
                LocalPosition = rightParent.localPosition,
                LocalRotation = rightParent.localRotation
            };
        }
    }
}