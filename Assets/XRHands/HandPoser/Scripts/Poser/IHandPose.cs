using System.Collections;
using System.Collections.Generic;
using InteractionsToolkit.Poser;
using UnityEngine;

public interface IHandPose
{
    public PoseData PrimaryPose { get; set; }
    public PoseData SecondaryPose { get; set; }

}
