using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldInPlaceTuningGame : TuningGame
{
    [SerializeField] private float holdTime = .5f;
    private bool completeHolding;
    [SerializeField] private GameObject correctShowObj;

    [Header("Coroutines")]
    private IEnumerator checkCorrect;

    protected virtual void Start()
    {
        completeHolding = false;
        ShowCorrect(false);
    }

    public override void SetGameOpen(bool open)
    {
        base.SetGameOpen(open);
        completeHolding = false;
        if (checkCorrect != null)
        {
            StopCoroutine(checkCorrect);
            checkCorrect = null;
        }
        if (open)
        {
            checkCorrect = CheckCorrect();
            StartCoroutine(checkCorrect);
            ShowCorrect(false);
        }
    }

    protected abstract bool CorrectSpot();

    private IEnumerator CheckCorrect()
    {
        while (true)
        {
            yield return new WaitUntil(() => CorrectSpot());
            IEnumerator waitCheck = WaitCheck();
            StartCoroutine(waitCheck);
            ShowCorrect(true);
            yield return new WaitUntil(() => completeHolding || !CorrectSpot());
            if (!CorrectSpot())
            {
                StopCoroutine(waitCheck);
                ShowCorrect(false);
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

    private void ShowCorrect(bool onTime)
    {
        correctShowObj.SetActive(onTime);
    }
}
