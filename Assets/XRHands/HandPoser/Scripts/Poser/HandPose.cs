using System.Collections;
using System.Collections.Generic;
using InteractionsToolkit.Poser;
using UnityEngine;

public class HandPose : MonoBehaviour, IHandPose
{
    PoseData primaryPose;
    PoseData secondaryPose;

    public PoseData PrimaryPose { get => primaryPose; set => primaryPose = value; }
    public PoseData SecondaryPose { get => secondaryPose; set => SecondaryPose = value; }
}
