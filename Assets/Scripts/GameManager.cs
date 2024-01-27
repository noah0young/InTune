using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    [Serializable]
    private class InstrumentInfo
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite requestSprite;
        [SerializeField] private Transform offscreenStart;
        [SerializeField] private Transform offscreenEnd;

        public Instrument Create(Transform parent)
        {
            GameObject instance = Instantiate(prefab, parent);
            return instance.GetComponent<Instrument>();
        }

        public Transform OffscreenStart()
        {
            return offscreenStart;
        }
        public Transform OffscreenEnd()
        {
            return offscreenEnd;
        }
    }

    public static GameManager Instance { get; private set; }

    private InputController input;

    [Header("Instruments")]
    [SerializeField] private List<InstrumentInfo> allInstruments;
    private int instrumentIndex = 0;
    private InstrumentInfo curInstrumentInfo = null;
    private Instrument curInstrument;

    [Header("Instrument Movement")]
    [SerializeField] private Transform instrumentParent;
    [SerializeField] private float instrumentMoveTime = 1;
    [SerializeField] private Transform instrumentPos;
    [SerializeField] private GameObject thanksObj;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetUpInput();
        thanksObj.SetActive(false);
    }

    private void SetUpInput()
    {
        input = new InputController();
        input.Game.Click.Enable();
    }

    private void Start()
    {
        StartCoroutine(GameRunner());
    }

    private IEnumerator GameRunner()
    {
        while (instrumentIndex < allInstruments.Count)
        {
            yield return SwitchInstruments(allInstruments[instrumentIndex]);
            yield return new WaitUntil(() => curInstrument.IsTuned());
            instrumentIndex++;
        }
    }

    private IEnumerator SwitchInstruments(InstrumentInfo newInstrument)
    {
        if (curInstrument != null)
        {
            StartCoroutine(MoveInstrument(instrumentPos, curInstrumentInfo.OffscreenEnd(), instrumentMoveTime, curInstrument, true));
            thanksObj.SetActive(true);
        }
        curInstrument = newInstrument.Create(instrumentParent);
        curInstrumentInfo = newInstrument;
        yield return StartCoroutine(MoveInstrument(curInstrumentInfo.OffscreenStart(), instrumentPos, instrumentMoveTime, curInstrument));
        thanksObj.SetActive(false);
    }

    private IEnumerator MoveInstrument(Transform start, Transform end, float time, Instrument newInstrument)
    {
        yield return MoveInstrument(start, end, time, newInstrument, false);
    }

    private IEnumerator MoveInstrument(Transform start, Transform end, float time, Instrument instrument, bool destroyAtEnd)
    {
        float timePassed = 0;
        while (timePassed < time)
        {
            timePassed += Time.deltaTime;
            instrument.transform.position = Vector3.Lerp(start.position, end.position, timePassed / time);
            instrument.transform.localScale = Vector3.Lerp(start.localScale, end.localScale, timePassed / time);
            instrument.transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, timePassed / time);
            yield return new WaitForEndOfFrame();
        }
        if (destroyAtEnd)
        {
            instrument.Destroy();
        }
    }

    public static bool NotClick()
    {
        return !Click();
    }

    public static bool Click()
    {
        return Instance.input.Game.Click.IsPressed();
    }
}
