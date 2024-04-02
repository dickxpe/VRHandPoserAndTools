// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

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