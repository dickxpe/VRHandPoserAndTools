// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using System;
using UltEvents;
using UnityEngine;

public class XRLever : MonoBehaviour
{
    [SerializeField]
    UltEvent OnEvent;
    [SerializeField]
    UltEvent OffEvent;
    [Serializable]
    public class FloatEvent : UltEvent<float> { }
    [SerializeField]
    FloatEvent valueChangedEvent;
    [SerializeField]
    float treshold = 10;
    [SerializeField]
    public bool isToggle = true;
    HingeJoint hinge;
    public float value;
    float max;
    float min;
    bool isOn = false;
    bool isOff = false;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponentInChildren<HingeJoint>();
        max = hinge.limits.max;
        min = hinge.limits.min;
    }

    // Update is called once per frame
    void Update()
    {
        if (isToggle)
        {
            if (hinge.angle > max - treshold)
            {
                if (!isOn)
                {
                    isOn = true;
                    OnEvent.Invoke();
                }
            }
            else
            {
                isOn = false;
            }

            if (hinge.angle < min + treshold)
            {
                if (!isOff)
                {
                    isOff = true;
                    OffEvent.Invoke();
                }
            }
            else
            {
                isOff = false;
            }
        }

        value = (Mathf.Abs(min) + hinge.angle) / (max - min);
        valueChangedEvent.Invoke(value);
    }
}
