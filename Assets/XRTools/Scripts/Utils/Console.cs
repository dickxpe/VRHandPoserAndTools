using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Console : MonoBehaviour
{
    public void Log(string message)
    {
        Debug.Log(message);
    }

    public void Log(int message)
    {
        Debug.Log(message.ToString());
    }

    public void Log(float message)
    {
        Debug.Log(message.ToString());
    }

    public void Log(Component component)
    {
        Debug.Log(component.ToString());
    }

    public static void Log(GameObject gameObject)
    {
        Debug.Log(gameObject.ToString());
    }



}
