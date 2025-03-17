using System;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvents : MonoBehaviour
{

    [Serializable]
    public class ColliderEvent : UltEvent<Collider> { }

    [Serializable]
    public class CollisionEvent : UltEvent<Collision> { }

    [SerializeField]
    public ColliderEvent onTriggerEnter;
    [SerializeField]
    public ColliderEvent onTriggerStay;
    [SerializeField]
    public ColliderEvent onTriggerExit;
    [SerializeField]
    public CollisionEvent onCollisionEnter;
    [SerializeField]
    public CollisionEvent onCollisionStay;
    [SerializeField]
    public CollisionEvent onCollisionExit;
    [SerializeField]
    UltEvent callbackEvent;


    public enum IncludeExcludeColliders
    {
        Include,
        Exclude
    }
    [SerializeField]
    IncludeExcludeColliders includeOrExcludeColliders = IncludeExcludeColliders.Exclude;
    [SerializeField]
    List<Collider> colliders = new List<Collider>();

    public void OnTriggerEnter(Collider other)
    {
        CheckTriggerCollider(other, onTriggerEnter);
    }


    public void OnTriggerStay(Collider other)
    {
        CheckTriggerCollider(other, onTriggerStay);
    }

    public void OnTriggerExit(Collider other)
    {
        CheckTriggerCollider(other, onTriggerExit);
    }

    public void OnCollisionEnter(Collision collision)
    {
        CheckCollisionCollider(collision, onCollisionEnter);
    }

    public void OnCollisionStay(Collision collision)
    {
        CheckCollisionCollider(collision, onCollisionStay);
    }

    public void OnCollisionExit(Collision collision)
    {
        CheckCollisionCollider(collision, onCollisionExit);
    }

    public void CallBack(bool status)
    {
        callbackEvent.Invoke();
    }

    public void CheckCollisionCollider(Collision collision, CollisionEvent callEvent)
    {
        if (includeOrExcludeColliders == IncludeExcludeColliders.Include)
        {
            if (colliders.Count > 0)
            {
                if (colliders.Contains(collision.collider))
                {
                    callEvent.Invoke(collision);
                }
            }
        }
        else if (includeOrExcludeColliders == IncludeExcludeColliders.Exclude)
        {
            if (colliders.Count > 0)
            {
                if (!colliders.Contains(collision.collider))
                {
                    callEvent.Invoke(collision);
                }
            }
            else
            {
                callEvent.Invoke(collision);
            }
        }
    }

    public void CheckTriggerCollider(Collider collider, ColliderEvent callEvent)
    {
        if (includeOrExcludeColliders == IncludeExcludeColliders.Include)
        {
            if (colliders.Count > 0)
            {
                if (colliders.Contains(collider))
                {
                    callEvent.Invoke(collider);
                }
            }
        }
        else if (includeOrExcludeColliders == IncludeExcludeColliders.Exclude)
        {
            if (colliders.Count > 0)
            {
                if (!colliders.Contains(collider))
                {
                    callEvent.Invoke(collider);
                }
            }
            else
            {
                callEvent.Invoke(collider);
            }
        }
    }
}
