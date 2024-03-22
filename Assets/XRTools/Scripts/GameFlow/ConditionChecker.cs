using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionsChecker : MonoBehaviour
{
    [SerializeField]
    bool[] conditions;

    bool allConditionsCompleted;

    [SerializeField]
    UnityEvent completedEvent;

    public void CompleteCondition(int id)
    {
        if (!allConditionsCompleted)
        {
            if (id < conditions.Length)
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
            allConditionsCompleted = true;
            completedEvent.Invoke();
        }
    }
}
