// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedrick

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