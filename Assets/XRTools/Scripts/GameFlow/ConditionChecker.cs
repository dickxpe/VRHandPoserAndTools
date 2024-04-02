// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UltEvents;
using UnityEngine;
public class ConditionsChecker : MonoBehaviour
{
    [SerializeField]
    bool[] conditions;
    bool allConditionsCompleted;
    [SerializeField]
    UltEvent completedEvent;

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
