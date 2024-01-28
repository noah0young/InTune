using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGame : TuningGame
{
    private bool paused;

    [Header("Transforms")]
    [SerializeField] private RectTransform timingBack;
    [SerializeField] private RectTransform targetRange;
    [SerializeField] private RectTransform cursor;

    [Header("Params")]
    [SerializeField] private float targetRangePercent = .1f;
    [SerializeField] private float speed = .5f;
    [SerializeField] private float cursorSize = 0.02f;

    [Header("Cursor Movement")]
    private float rangeTop;
    private float rangeBottom;
    private bool movingUp = true;
    private float cursorPos = .5f;

    [Header("Target Possible Range")]
    [SerializeField] private float targetRangeMax = 1f;
    [SerializeField] private float targetRangeMin = 0f;

    [Header("On Click")]
    [SerializeField] private GameObject successPrefab;
    [SerializeField] private GameObject failPrefab;
    [SerializeField] private float showSuccessFailTime = 1;

    private void Start()
    {
        paused = false;
        SetTargetRange();
        StartCoroutine(WaitForClick());
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
        if (!paused)
        {
            MoveCursor();
        }
    }

    private void MoveCursor()
    {
        float direction = movingUp ? 1 : -1;
        cursorPos += direction * speed * Time.deltaTime;
        if (cursorPos >= 1)
        {
            cursorPos = 1;
            movingUp = !movingUp;
        }
        else if (cursorPos <= 0)
        {
            cursorPos = 0;
            movingUp = !movingUp;
        }
        SetCursorPos(cursorPos);
    }

    private bool OnTime()
    {
        return cursorPos > rangeBottom && cursorPos < rangeTop;
    }

    private IEnumerator WaitForClick()
    {
        while (!IsTuned())
        {
            yield return new WaitUntil(() => GameManager.NotClick());
            yield return new WaitUntil(() => GameManager.Click());
            paused = true;
            GameObject successFailPrefab = OnTime() ? successPrefab : failPrefab;
            if (successFailPrefab != null)
            {
                Destroy(Instantiate(successFailPrefab), showSuccessFailTime);
            }
            yield return new WaitForSeconds(showSuccessFailTime);
            if (OnTime())
            {
                OnTuned();
            }
            else
            {
                paused = false;
            }
        }
    }
}
