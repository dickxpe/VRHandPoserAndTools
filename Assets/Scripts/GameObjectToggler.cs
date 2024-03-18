using System.Collections;
using System.Collections.Generic;
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
