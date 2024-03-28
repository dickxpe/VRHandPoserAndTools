using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour, ICallbackEvent
{

    [SerializeField]
    UltEvent onCollisionEnter;
    [SerializeField]
    UltEvent onCollisionStay;
    [SerializeField]
    UltEvent onCollisionExit;

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

    void OnCollisionEnter(Collision collision)
    {
        CheckCollider(collision.collider, onCollisionEnter);
    }


    void OnCollisionStay(Collision collision)
    {
        CheckCollider(collision.collider, onCollisionStay);
    }


    void OnCollisionExit(Collision collision)
    {
        CheckCollider(collision.collider, onCollisionExit);
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
