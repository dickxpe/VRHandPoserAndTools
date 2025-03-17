using System;
using System.Collections.Generic;
using UltEvents;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardEvents : MonoBehaviour
{
    [Serializable]
    public class StringEvent : UltEvent<String> { }

    public bool allKeys = true;
    public KeyCode keyCode;


    [SerializeField]
    public StringEvent onKeyDown;

    [SerializeField]
    public StringEvent onKeyUp;

    [SerializeField]
    public StringEvent onKey;

    protected List<KeyCode> activeInputs = new List<KeyCode>();

    public void Update()
    {
        if (allKeys)
        {

            List<KeyCode> pressedInput = new List<KeyCode>();

            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code)) // Use GetKeyDown for first press only
                    {
                        activeInputs.Remove(code);
                        activeInputs.Add(code);
                        pressedInput.Add(code);

                        onKeyDown.Invoke(code.ToString()); // Invoke only on first press
                    }
                }
            }

            // Invoke onKeyHold when the key is continuously held down
            foreach (KeyCode code in activeInputs)
            {
                if (Input.GetKey(code) && !pressedInput.Contains(code))
                {
                    onKey.Invoke(code.ToString()); // Invoke onKey while key is held
                }
            }

            // Track released keys
            List<KeyCode> releasedInput = new List<KeyCode>();

            foreach (KeyCode code in activeInputs)
            {
                if (!Input.GetKey(code)) // Key is no longer held
                {
                    onKeyUp.Invoke(code.ToString()); // Invoke on release
                }
                else
                {
                    releasedInput.Add(code); // Keep tracking active keys
                }
            }

            activeInputs = releasedInput;

        }
        else
        {

            if (Input.GetKeyDown(keyCode))
            {
                onKeyDown.Invoke(keyCode.ToString());
            }
            if (Input.GetKeyUp(keyCode))
            {
                onKeyUp.Invoke(keyCode.ToString());
            }
            if (Input.GetKey(keyCode))
            {
                onKey.Invoke(keyCode.ToString());
            }
        }
    }
}
