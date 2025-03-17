
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UltEvents;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{


    [SerializeField]
    [SerializedDictionary("Event name", "Actions")]
    public SerializedDictionary<string, UltEvent> animationEvents;


    public void AnimationEvent(string eventName)
    {
        UltEvent onAnimationEvent;
        if (animationEvents.TryGetValue(eventName, out onAnimationEvent))
        {
            onAnimationEvent.Invoke();
        }
        else
        {
            Debug.LogError("No event configured for animation event '" + eventName + "'");
        }
    }

}