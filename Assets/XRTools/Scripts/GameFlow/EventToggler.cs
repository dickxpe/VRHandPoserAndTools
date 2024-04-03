// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UltEvents;
using UnityEngine;

public class EventToggler : MonoBehaviour
{
    [SerializeField]
    UltEvent trueEvent;
    [SerializeField]
    UltEvent falseEvent;

    public bool toggle = false;

    public void Toggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            trueEvent.Invoke();
        }
        else
        {
            falseEvent.Invoke();
        }
    }
    public void TriggerTrue()
    {
        toggle = true;
        trueEvent.Invoke();
    }
    public void TriggerFalse()
    {
        toggle = false;
        falseEvent.Invoke();
    }
}
