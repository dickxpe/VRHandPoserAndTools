using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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