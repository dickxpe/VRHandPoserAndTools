using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Pooling;
#if BURST_PRESENT
using Unity.Burst;
#endif

public partial class XRPoseGrabInteractable : XRGrabInteractable
{
    protected override void SetupRigidbodyDrop(Rigidbody rigidbody)
    {
        base.SetupRigidbodyDrop(rigidbody);
        // Remember Rigidbody settings and setup to move
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }

    protected override void Grab()
    {

    }

    protected override void Drop()
    {
        base.Drop();
        transform.SetParent(null);
    }
}