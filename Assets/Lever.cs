using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnEvent;
    [SerializeField]
    UnityEvent OffEvent;
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    public FloatEvent valueChangedEvent;

    [SerializeField]
    float treshold = 10;

    HingeJoint hinge;

    [SerializeField]
    public bool isToggle = true;

    public float value;

    float max;
    float min;

    bool isOn = false;
    bool isOff = false;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        max = hinge.limits.max;
        min = hinge.limits.min;

    }

    // Update is called once per frame
    void Update()
    {
        valueChangedEvent.Invoke(value);
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


    }
}
