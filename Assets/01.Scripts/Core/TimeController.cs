using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Timecontroller is running");
            Destroy(this); //있으면 일단 파괴
        }
        instance = this;
    }

    public void ResetTimeScale()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }

    public void ModifyTimeScale(float endTimeValue, float timeToWait, Action OnCompleteHandler = null)
    {
        StartCoroutine(TimeScaleCoroutine(endTimeValue, timeToWait, OnCompleteHandler));
    }

    IEnumerator TimeScaleCoroutine(float endTimeValue, float timeToWait, Action OnCompleteHandler = null)
    {
        //코루틴의 RealTime 은 대기시간은 TimeScale에 영향받지 않는다.
        yield return new WaitForSecondsRealtime(timeToWait);
        Time.timeScale = endTimeValue;
        OnCompleteHandler?.Invoke();
    }
}
