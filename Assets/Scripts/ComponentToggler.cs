using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentToggler : MonoBehaviour
{
    [SerializeField]
    Behaviour component;

    bool isOn = false;
    void Awake()
    {
        isOn = component.enabled;
    }
    public void Toggle()
    {
        isOn = !isOn;
        component.enabled = isOn;
    }

}
