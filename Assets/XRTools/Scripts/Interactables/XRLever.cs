using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class XRLever : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnEvent;

    [SerializeField]
    UnityEvent OffEvent;

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

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

                if (hinge.angle >= max)
                {

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

                if (hinge.angle <= min)
                {

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
