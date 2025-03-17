using System;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionEvents : MonoBehaviour
{
    [Serializable]
    public class InputEventFloat : UltEvent<float> { }

    [Serializable]
    public class InputEventVector2 : UltEvent<Vector2> { }

    [Serializable]
    public class InputEvent
    {
        public InputActionReference action;

        public UltEvent onStarted;
        public InputEventVector2 onPerformed;
        public UltEvent onCanceled;
    }

    [SerializeField] private InputEvent[] inputEvents;

    private void OnEnable()
    {
        foreach (var inputEvent in inputEvents)
        {
            if (inputEvent.action == null) continue;

            inputEvent.action.action.started += ctx => inputEvent.onStarted.Invoke();

            inputEvent.action.action.performed += ctx =>
            {
                if (ctx.action.expectedControlType == "Vector2")
                    inputEvent.onPerformed.Invoke(ctx.ReadValue<Vector2>());
                else if (ctx.action.expectedControlType == "Axis" || ctx.action.expectedControlType == "Button")
                    inputEvent.onPerformed.Invoke(new Vector2(ctx.ReadValue<float>(), 0));
            };

            inputEvent.action.action.canceled += ctx => inputEvent.onCanceled.Invoke();

            inputEvent.action.action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var inputEvent in inputEvents)
        {
            if (inputEvent.action == null) continue;

            inputEvent.action.action.started -= ctx => inputEvent.onStarted.Invoke();

            inputEvent.action.action.performed -= ctx =>
            {
                if (ctx.action.expectedControlType == "Vector2")
                    inputEvent.onPerformed.Invoke(ctx.ReadValue<Vector2>());
                else if (ctx.action.expectedControlType == "Axis" || ctx.action.expectedControlType == "Button")
                    inputEvent.onPerformed.Invoke(new Vector2(ctx.ReadValue<float>(), 0));
            };

            inputEvent.action.action.canceled -= ctx => inputEvent.onCanceled.Invoke();

            inputEvent.action.action.Disable();
        }
    }
}
