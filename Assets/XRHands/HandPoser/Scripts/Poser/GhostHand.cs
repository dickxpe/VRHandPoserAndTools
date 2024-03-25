using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InteractionsToolkit.Poser
{
    public class GhostHand : MonoBehaviour
    {
        [HideInInspector]
        public Transform followObject;

        public void Update()
        {
            if (followObject)
            {
                transform.position = followObject.position;
                transform.rotation = followObject.rotation;

            }
        }

    }
}