// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using System.Collections.Generic;

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