// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using System.Collections.Generic;

namespace InteractionsToolkit.Poser
{
    [System.Serializable]
    public class HandPoseJoints
    {
        public HandPoseJoints()
        {
            poseJointGroups = new List<HandPoseJointGroup>();
        }

        public List<HandPoseJointGroup> poseJointGroups;

        public int GetTotalJointCount()
        {

            int totalJoints = 0;
            for (int i = 0; i < poseJointGroups.Count; i++)
            {
                totalJoints += poseJointGroups[i].poseJoints.Count;
            }
            return totalJoints;
        }

    }
}