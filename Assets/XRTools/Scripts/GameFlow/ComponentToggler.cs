// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

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
