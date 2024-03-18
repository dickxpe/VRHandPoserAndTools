using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [System.Serializable]
    public class ButtonEvent : UnityEvent { }

    public float maxPress;

    public float pressTreshold;
    public bool pressed;
    public UnityEvent downEvent;

    public float distance;

    Vector3 startPos;

    Transform knob;

    void Start()
    {
        knob = transform.Find("Knob").transform;
        startPos = knob.position;

    }

    void Update()
    {
        // If our distance is greater than what we specified as a press
        // set it to our max distance and register a press if we haven't already
        distance = Mathf.Abs(knob.position.y - startPos.y);
        if (distance >= maxPress)
        {
            // Prevent the button from going past the pressLength
            knob.position = new Vector3(knob.position.x, startPos.y - maxPress, knob.position.z);
            if (!pressed && distance >= pressTreshold)
            {
                pressed = true;
                // If we have an event, invoke it
                downEvent?.Invoke();
            }
        }
        else if (distance < pressTreshold)
        {
            // If we aren't all the way down, reset our press
            pressed = false;
        }
        // Prevent button from springing back up past its original position
        if (knob.position.y > startPos.y)
        {
            knob.position = new Vector3(knob.position.x, startPos.y, knob.position.z);
        }
    }
}