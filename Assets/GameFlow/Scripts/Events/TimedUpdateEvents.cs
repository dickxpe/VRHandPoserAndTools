using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class TimedUpdateEvents : MonoBehaviour
{
    [SerializeField]
    UltEvent onInterval;


    float timePassed = 0;

    public float interval = 1f;

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= interval)
        {
            timePassed = 0;
            onInterval.Invoke();
        }
    }


}

