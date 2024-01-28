using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] protected SevenSegmentDisplayManager hour10s;
    [SerializeField] protected SevenSegmentDisplayManager hour1s;
    [SerializeField] protected SevenSegmentDisplayManager min10s;
    [SerializeField] protected SevenSegmentDisplayManager min1s;

    private void Update()
    {
        UpdateTime();
    }

    protected virtual void UpdateTime()
    {
        int hour = DateTime.Now.Hour;
        hour10s.SetNumber(hour % 12 / 10);
        hour1s.SetNumber(hour % 12 % 10);
        int min = DateTime.Now.Minute;
        min10s.SetNumber(min / 10);
        min1s.SetNumber(min % 10);
    }

    public static int CurHour()
    {
        int hour = DateTime.Now.Hour;
        return hour % 12;
    }

    public static int CurMin()
    {
        int min = DateTime.Now.Minute;
        return min;
    }
}
