using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TirePumpGame : HoldInPlaceTuningGame
{
    [SerializeField] private bool broken = false;

    [Header("Transforms")]
    [SerializeField] private RectTransform timingBack;
    [SerializeField] private RectTransform targetRange;
    [SerializeField] private RectTransform cursor;

    [Header("Target Possible Range")]
    [SerializeField] private float targetRangeMax = 0.8f;
    [SerializeField] private float targetRangeMin = 0.2f;

    [Header("Params")]
    [SerializeField] private float targetRangePercent = .1f;
    [SerializeField] private float cursorSize = 0.02f;
    [SerializeField] private float gravityRate = .1f;
    [SerializeField] private float pumpSpeedMax = .3f;
    [SerializeField] private float pumpSpeedMin = 0.3f;
    [SerializeField] private float maxCursorSpeed = .4f;

    [Header("Cursor Movement")]
    private float cursorSpeed = 0;
    private float rangeTop;
    private float rangeBottom;
    private float cursorPos = 0f;

    public override void SetGameOpen(bool open)
    {
        StopAllCoroutines();
        base.SetGameOpen(open);
        if (open)
        {
            SetTargetRange();
            SetCursorPos(cursorPos);
            StartCoroutine(WaitForClick());
        }
    }

    private void SetTargetRange()
    {
        float targetRangeTop = Random.Range(targetRangePercent + targetRangeMin, targetRangeMax);
        rangeTop = targetRangeTop;
        rangeBottom = rangeTop - targetRangePercent;
        targetRange.anchorMax = new Vector2(targetRange.anchorMax.x, targetRangeTop);
        targetRange.anchorMin = new Vector2(targetRange.anchorMin.x, targetRangeTop - targetRangePercent);
    }

    private void SetCursorPos(float percent)
    {
        percent = percent / (1 + cursorSize);
        percent += cursorSize / 2;
        cursor.anchorMax = new Vector2(cursor.anchorMax.x, percent + cursorSize / 2);
        cursor.anchorMin = new Vector2(cursor.anchorMin.x, percent - cursorSize / 2);
    }

    private void Update()
    {
        UpdateCursor();
        ApplyGravity();
    }

    private void UpdateCursor()
    {
        cursorPos += cursorSpeed * Time.deltaTime;
        if (cursorPos <= 0)
        {
            cursorSpeed = 0;
            cursorPos = 0;
        }
        else if (cursorPos >= 1 && !broken)
        {
            cursorSpeed = 0;
            cursorPos = 1;
        }
        SetCursorPos(cursorPos);
    }

    private void ApplyGravity()
    {
        cursorSpeed -= gravityRate;
        cursorSpeed = Mathf.Max(cursorSpeed, -maxCursorSpeed);
    }

    private void Pump()
    {
        cursorSpeed += PumpIncSpeed();
        cursorSpeed = Mathf.Min(cursorSpeed, maxCursorSpeed);
    }

    private float PumpIncSpeed()
    {
        return Mathf.Lerp(pumpSpeedMax, pumpSpeedMin, cursorPos);
    }

    private bool OnTime()
    {
        return cursorPos > rangeBottom && cursorPos < rangeTop;
    }

    private IEnumerator WaitForClick()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.NotClick());
            yield return new WaitUntil(() => GameManager.Click());
            Pump();
        }
    }

    protected override bool CorrectSpot()
    {
        return cursorPos > rangeBottom && cursorPos < rangeTop;
    }
}
