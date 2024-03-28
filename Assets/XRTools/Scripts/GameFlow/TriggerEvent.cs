using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class TriggerEvent : MonoBehaviour, ICallbackEvent
{
    [SerializeField]
    UltEvent onTriggerEnter;

    [SerializeField]
    UltEvent onTriggerStay;

    [SerializeField]
    UltEvent onTriggerExit;

    [SerializeField]
    UltEvent callbackEvent;

    [SerializeField]
    IncludeExcludeColliders includeOrExcludeColliders = IncludeExcludeColliders.Exclude;

    [SerializeField]
    List<Collider> colliders = new List<Collider>();


    public enum IncludeExcludeColliders
    {
        Include,
        Exclude
    }

    void OnTriggerEnter(Collider other)
    {
        CheckCollider(other, onTriggerEnter);
    }

    void OnTriggerStay(Collider other)
    {
        CheckCollider(other, onTriggerStay);
    }

    void OnTriggerExit(Collider other)
    {
        CheckCollider(other, onTriggerExit);
    }

    public void CheckCollider(Collider collider, UltEvent callEvent)
    {
        if (includeOrExcludeColliders == IncludeExcludeColliders.Include)
        {
            if (colliders.Count > 0)
            {
                if (colliders.Contains(collider))
                {
                    callEvent.Invoke();
                }
            }
        }
        else if (includeOrExcludeColliders == IncludeExcludeColliders.Exclude)
        {
            if (colliders.Count > 0)
            {
                if (!colliders.Contains(collider))
                {
                    callEvent.Invoke();
                }
            }
            else
            {
                callEvent.Invoke();
            }
        }
    }

    public void CallBack()
    {
        callbackEvent.Invoke();
    }
}
