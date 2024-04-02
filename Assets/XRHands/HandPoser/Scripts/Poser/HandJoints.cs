// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using System.Collections.Generic;

namespace InteractionsToolkit.Poser
{
    [System.Serializable]
    public class HandJoints
    {
        public HandJoints()
        {
            jointGroups = new List<HandJointGroup>();
        }


        public List<HandJointGroup> jointGroups;

        public int GetTotalJointCount()
        {

            int totalJoints = 0;
            for (int i = 0; i < jointGroups.Count; i++)
            {
                totalJoints += jointGroups[i].joints.Count;
            }
            return totalJoints;
        }

    }
}