// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;

public class GameObjectToggler : MonoBehaviour
{
    bool isOn = false;

    void Awake()
    {
        isOn = gameObject.activeSelf;
    }

    public void Toggle()
    {
        isOn = !isOn;
        gameObject.SetActive(isOn);
    }

}
