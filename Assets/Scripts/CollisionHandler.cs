using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    Collision collision;

    public void HandleCollisionEnter(Collision collision)
    {
        Debug.Log(collision.GetContact(0).point);
    }

}
