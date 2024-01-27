using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private SevenSegmentDisplayManager hour10s;
    [SerializeField] private SevenSegmentDisplayManager hour1s;
    [SerializeField] private SevenSegmentDisplayManager min10s;
    [SerializeField] private SevenSegmentDisplayManager min1s;

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
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
