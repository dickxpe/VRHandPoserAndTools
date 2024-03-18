using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InteractionsToolkit.Poser
{
    [System.Serializable]
    public class HandPoseJointGroup
    {
        public HandPoseJointGroup()
        {
            poseJoints = new List<PoseTransform>();
        }

        public List<PoseTransform> poseJoints;
    }
}