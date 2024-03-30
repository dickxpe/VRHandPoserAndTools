using System;
using System.Collections;
using System.Collections.Generic;
using InteractionsToolkit.Core;
using InteractionsToolkit.Poser;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct ActionPose
{
    public InputActionProperty action;
    public PoseData pose;
    public UnityEvent actionEvent;

    public UnityEvent undoActionEvent;


}
