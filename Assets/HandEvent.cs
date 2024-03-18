using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;

public class HandEvent : MonoBehaviour
{
    XRHandTrackingEvents events;

    void Start()
    {
        events = GetComponent<XRHandTrackingEvents>();
        events.poseUpdated.AddListener(GetPose);
        events.jointsUpdated.AddListener(GetJoints);
    }

    void GetPose(UnityEngine.Pose pose)
    {
        //Debug.Log(pose);
    }

    void GetJoints(XRHandJointsUpdatedEventArgs args)
    {
        XRDirectInteractor directInteractor;


    }
}
