// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx


using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InteractionsToolkit.Poser
{

    [Serializable]
    public struct ActionPose
    {
        public InputActionProperty action;
        public PoseData pose;
        public UnityEvent actionEvent;
        public UnityEvent undoActionEvent;
    }
}
