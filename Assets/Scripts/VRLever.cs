using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class VRLever : MonoBehaviour
{
    public UnityEvent LeverOn = new UnityEvent();
    public UnityEvent LeverOff = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ON")
        {
            LeverOn.Invoke();
        }
        else if (other.gameObject.name == "OFF")
        {
            LeverOff.Invoke();
        }
    }


}
