// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using System.Collections.Generic;
using UnityEngine;
namespace InteractionsToolkit.Poser
{
    [System.Serializable]
    public class HandJointGroup
    {
        public HandJointGroup()
        {
            joints = new List<Transform>();
        }

        public List<Transform> joints;
    }
}