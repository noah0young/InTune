using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchSetGame : HoldInPlaceTuningGame
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject hourHand;
    [SerializeField] private GameObject minHand;
    [SerializeField] private Slider slider;
    [SerializeField] private float minRange = 5;
    private float curMin;
    private float curHour;
    /*[SerializeField] private float holdTime = .5f;
    private bool completeHolding;
    [SerializeField] private GameObject onTimeShowObj;

    [Header("Coroutines")]
    private IEnumerator checkOnTime;*/

    protected override void Start()
    {
        mainCanvas.worldCamera = Camera.main;
        base.Start();
    }

    /*public override void SetGameOpen(bool open)
    {
        base.SetGameOpen(open);
        completeHolding = false;
        if (checkOnTime != null)
        {
            StopCoroutine(checkOnTime);
            checkOnTime = null;
        }
        if (open)
        {
            checkOnTime = CheckOnTime();
            StartCoroutine(checkOnTime);
            ShowOnTime(false);
        }
    }*/

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
        return (curMinInt > actualMin - minRange / 2 && curMinInt < actualMin + minRange / 2
            && curHourInt == actualHour);
    }

    /*private IEnumerator CheckOnTime()
    {
        while (true)
        {
            yield return new WaitUntil(() => OnTime());
            IEnumerator waitCheck = WaitCheck();
            StartCoroutine(waitCheck);
            ShowOnTime(true);
            yield return new WaitUntil(() => completeHolding || !OnTime());
            if (!OnTime())
            {
                StopCoroutine(waitCheck);
                ShowOnTime(false);
            }
            else
            {
                OnTuned();
            }
        }
    }

    private IEnumerator WaitCheck()
    {
        yield return new WaitForSeconds(holdTime);
        completeHolding = true;
    }

    private void ShowOnTime(bool onTime)
    {
        onTimeShowObj.SetActive(onTime);
    }*/
}
