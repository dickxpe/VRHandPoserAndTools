// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using InteractionsToolkit.Poser;
using UnityEngine;
namespace InteractionsToolkit.Poser
{
    public class HandPose : MonoBehaviour, IHandPose
    {
        PoseData primaryPose;
        PoseData secondaryPose;

        public PoseData PrimaryPose { get => primaryPose; set => primaryPose = value; }
        public PoseData SecondaryPose { get => secondaryPose; set => SecondaryPose = value; }
    }
}
