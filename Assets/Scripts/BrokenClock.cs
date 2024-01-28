using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenClock : Clock
{
    [SerializeField] private WatchSetGame game;
    [SerializeField] private int offByMin = 10;

    protected override void UpdateTime()
    {
        int hour = game.GetCurHour() + ((game.GetCurMin() + offByMin) / 60);
        hour10s.SetNumber(hour % 12 / 10);
        hour1s.SetNumber(hour % 12 % 10);
        int min = (game.GetCurMin() + offByMin) % 60;
        min10s.SetNumber(min / 10);
        min1s.SetNumber(min % 10);
    }
}
