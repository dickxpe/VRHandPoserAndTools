using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequenceChecker : MonoBehaviour
{

    [SerializeField]
    int[] sequence;

    [SerializeField]
    UnityEvent completedEvent;

    bool sequenceCompleted;

    int position = 0;

    public void CheckSequence(int id, GameObject gameObject)
    {
        if (!sequenceCompleted)
        {
            if (sequence[position] == id)
            {
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
