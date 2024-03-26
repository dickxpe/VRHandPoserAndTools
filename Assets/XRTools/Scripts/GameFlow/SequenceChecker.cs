using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class SequenceChecker : MonoBehaviour
{

    [SerializeField]
    int[] sequence;


    [SerializeField]
    UltEvent checkEvent;


    [SerializeField]
    UltEvent checkCorrectEvent;


    [SerializeField]
    UltEvent completedEvent;

    bool sequenceCompleted;

    int position = 0;

    public void CheckSequence(int id, GameObject gameObject)
    {
        if (!sequenceCompleted)
        {
            checkEvent.Invoke();
            if (sequence[position] == id)
            {
                checkCorrectEvent.Invoke();
                gameObject.GetComponent<ICallbackEvent>().CallBack();
                position++;
                if (position == sequence.Length)
                {
                    completedEvent.Invoke();
                    sequenceCompleted = true;
                }
            }
        }
    }

}
