using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchSetGame : HoldInPlaceTuningGame
{
    [SerializeField] private bool broken = false;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject hourHand;
    [SerializeField] private GameObject minHand;
    [SerializeField] private Slider slider;
    [SerializeField] private float minRange = 5;
    private float curMin;
    private float curHour;

    protected override void Start()
    {
        mainCanvas.worldCamera = Camera.main;
        base.Start();
    }

    private void Update()
    {
        SetTimeToSlider();
    }

    private void SetTimeToSlider()
    {
        float timePercent = slider.value;
        float timeInMin = Mathf.Lerp(0, 720, timePercent);
        curMin = timeInMin % 60;
        curHour = timeInMin / 60 % 12;
        hourHand.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -360, curHour / 12));
        minHand.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -360, curMin / 60));
    }

    protected override bool CorrectSpot()
    {
        int actualMin = Clock.CurMin();
        int actualHour = Clock.CurHour();
        int curMinInt = (int)curMin;
        int curHourInt = (int)curHour;
        return !broken && (curMinInt > actualMin - minRange / 2 && curMinInt < actualMin + minRange / 2
            && curHourInt == actualHour);
    }

    public int GetCurMin()
    {
        return (int)curMin;
    }

    public int GetCurHour()
    {
        return (int)curHour;
    }
}
