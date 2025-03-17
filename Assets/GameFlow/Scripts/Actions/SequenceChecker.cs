// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;
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
    UltEvent checkWrongEvent;
    [SerializeField]
    UltEvent completedEvent;
    bool sequenceCompleted;
    int position = 0;

    public void CheckSequence(int id)
    {
        CheckSequence(id, null);
    }

    public void CheckSequence(int id, GameObject gameObject)
    {
        if (!sequenceCompleted)
        {
            checkEvent.Invoke();
            if (sequence[position] == id)
            {
                checkCorrectEvent.Invoke();
                if (gameObject != null)
                {
                    gameObject.GetComponent<ICallbackEvent>().CallBack(true);
                }
                position++;
                if (position == sequence.Length)
                {
                    completedEvent.Invoke();
                    sequenceCompleted = true;
                }
            }
            else
            {
                checkWrongEvent.Invoke();
                if (gameObject != null)
                {
                    gameObject.GetComponent<ICallbackEvent>().CallBack(false);
                }
            }
        }
    }
}
