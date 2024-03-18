using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent onTriggerEnter;
    [SerializeField]
    UnityEvent onTriggerStay;
    [SerializeField]
    UnityEvent onTriggerExit;

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
