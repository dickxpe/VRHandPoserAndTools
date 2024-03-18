using System.Collections;
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