using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class DelayEvents : MonoBehaviour
{

    float delay = 1f;

    [SerializeField]
    UltEvent afterDelay;

    public void Delay(float seconds)
    {
        delay = seconds;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        afterDelay.Invoke();
    }

}

