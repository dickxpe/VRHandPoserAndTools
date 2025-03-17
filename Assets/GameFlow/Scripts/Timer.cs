using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UltEvents;

public class Timer : MonoBehaviour
{
    [SerializeField]
    bool visibleTimer = true;

    [SerializeField]
    bool startOnAwake;

    bool hasStarted;

    [SerializeField]
    float intervalMin = 0.5f;

    [SerializeField]
    float intervalMax = 2f;

    float interval;

    [SerializeField]
    bool countDown = true;

    bool delayStarted = false;

    [SerializeField]
    float duration = 10;

    [SerializeField]
    float startDelay = 0;

    float timeCounter;

    float startDelayCounter = 0;

    public TMP_Text timerText;

    [SerializeField]
    UltEvent onTimerStarted;

    [SerializeField]
    UltEvent onTimerCompleted;

    [SerializeField]
    FloatEvent onIntervalHasPassed;

    float intervalCounter = 0;
    float milliSecondsPassed = 0;

    [System.Serializable]
    public class FloatEvent : UltEvent<float> { }

    // Start is called before the first frame update
    void Start()
    {

        interval = Random.Range(intervalMin, intervalMax);

        if (visibleTimer)
        {

            timerText.enabled = true;
            timerText.text = "00:00.000";
        }
        else
        {
            timerText.enabled = false;
        }

        if (startOnAwake)
        {
            StartTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            if (startDelayCounter < startDelay)
            {
                startDelayCounter += Time.deltaTime;
            }
            else
            {

                if (!delayStarted)
                {
                    delayStarted = true;
                    onTimerStarted.Invoke();
                }

                intervalCounter += Time.deltaTime;
                if (intervalCounter >= interval)
                {
                    milliSecondsPassed += intervalCounter;
                    intervalCounter = 0;
                    interval = Random.Range(intervalMin, intervalMax);
                    onIntervalHasPassed.Invoke(milliSecondsPassed);

                }
                if (countDown)
                {
                    timeCounter -= Time.deltaTime;
                    if (timeCounter <= 0)
                    {
                        onTimerCompleted.Invoke();
                        timeCounter = 0;
                        hasStarted = false;
                    }
                }
                else
                {
                    timeCounter += Time.deltaTime;
                    if (timeCounter >= duration)
                    {
                        onTimerCompleted.Invoke();
                        timeCounter = duration;
                        hasStarted = false;
                    }
                }
                timerText.text = System.TimeSpan.FromSeconds(timeCounter).ToString("mm\\:ss\\.fff");
            }

        }

    }

    public void StartTimer()
    {

        hasStarted = true;
        if (countDown)
        {
            timeCounter = duration;
        }
        else
        {
            timeCounter = 0;
        }
    }

    public void StartTimer(float duration)
    {
        StartTimer();
    }
}
