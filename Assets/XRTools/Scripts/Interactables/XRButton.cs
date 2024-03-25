using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class XRButton : MonoBehaviour, ICallbackEvent
{

    [SerializeField]
    float pressTreshold;

    [SerializeField]
    float maxPress;

    [SerializeField]
    UltEvent downEvent;

    [SerializeField]
    UltEvent upEvent;

    [SerializeField]
    UltEvent callbackEvent;

    bool pressed;

    float distance;

    Vector3 startPos;

    Transform knob;

    bool upEventCanTrigger = false;

    void Start()
    {
        knob = transform.Find("Knob").transform;
        startPos = knob.localPosition;
    }

    void FixedUpdate()
    {
        knob.localPosition = new Vector3(0, knob.localPosition.y, 0);

        distance = Mathf.Abs(knob.localPosition.y - startPos.y);
        if (distance > pressTreshold)
        {

            upEventCanTrigger = true;
            if (distance >= maxPress)
            {
                knob.localPosition = new Vector3(0, startPos.y - maxPress, 0);
            }
            if (!pressed)
            {
                pressed = true;
                downEvent?.Invoke();
            }
        }
        else if (distance < pressTreshold)
        {
            pressed = false;
            if (upEventCanTrigger)
            {
                upEvent?.Invoke();
                upEventCanTrigger = false;
            }
        }

        if (knob.localPosition.y > startPos.y)
        {
            knob.localPosition = new Vector3(0, startPos.y, 0);
        }
    }

    public void CallBack()
    {
        callbackEvent.Invoke();
    }
}