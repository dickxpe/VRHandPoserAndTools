using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField]
    UltEvent onCollisionEnter;
    [SerializeField]
    UltEvent onCollisionStay;
    [SerializeField]
    UltEvent onCollisionExit;


    void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke();
        CollisionHandler collisionHandler = GetComponent<CollisionHandler>();
        if (collisionHandler != null)
        {
            collisionHandler.HandleCollisionEnter(collision);
        }
    }


    void OnCollisionStay(Collision collision)
    {
        onCollisionStay.Invoke();
    }


    void OnCollisionExit(Collision collision)
    {
        onCollisionExit.Invoke();
    }
}
