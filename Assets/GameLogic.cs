using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    List<bool> conditions = new List<bool>();

    [SerializeField]
    UnityEvent completedEvent;

    public void CompleteCondition(int id)
    {
        if (id < conditions.Count)
        {
            conditions[id] = true;
        }

        foreach (bool condition in conditions)
        {
            if (!condition)
            {
                return;
            }
        }
        completedEvent.Invoke();
    }
}
