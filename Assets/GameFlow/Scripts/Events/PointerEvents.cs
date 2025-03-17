using System;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{

    [Serializable]
    public class Vector3Event : UltEvent<Vector3> { }

    public Vector3Event onPointerClick;

    public Vector3Event onPointerDoubleClick;

    public UltEvent onPointerDown;
    public UltEvent onPointerUp;
    public UltEvent onPointerEnter;
    public UltEvent onPointerExit;
    public UltEvent onPointerMove;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) // Detect double-click
        {
            onPointerDoubleClick.Invoke(eventData.pointerPressRaycast.worldPosition);
        }
        else
        {
            onPointerClick.Invoke(eventData.pointerPressRaycast.worldPosition);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove.Invoke();
    }


}
