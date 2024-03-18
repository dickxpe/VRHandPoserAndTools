using System;
using UnityEngine;

namespace InteractionsToolkit.Poser
{
    [Serializable]
    public struct PoseTransform
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
    }
}