using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    UltEvent onTriggerEnter;

    [SerializeField]
    UltEvent onTriggerStay;

    [SerializeField]
    UltEvent onTriggerExit;

    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }
}
