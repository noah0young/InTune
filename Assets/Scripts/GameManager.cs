//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using System.Collections;

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

    private float fullTimeTaken = 0;
    [SerializeField] private TMP_Text speedRunningClockText;
    [SerializeField] private TMP_Text numCompletedText;

    [Header("Instruments")]
    [SerializeField] private List<InstrumentInfo> allInstruments;
    private int instrumentIndex = 0;
    private InstrumentInfo curInstrumentInfo = null;
    private Instrument curInstrument;
    [SerializeField] private GameObject hideWhenMinigame;

    [Header("Instrument Movement")]
    [SerializeField] private Transform instrumentParent;
    [SerializeField] private float instrumentMoveTime = 1;
    [SerializeField] private Transform instrumentPos;
    [SerializeField] private GameObject thanksObj;

    [Header("End")]
    [SerializeField] private float nextSceneTimer = 120;
    [SerializeField] private string nextScene = "FailNarrate";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetUpInput();
        ShowThanks(false);
    }

    private void ShowThanks(bool show)
    {
        if (thanksObj != null)
        {
            thanksObj.SetActive(show);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void SetUpInput()
    {
        input = new InputController();
        input.Game.Click.Enable();
        Instance.input.Game.MousePos.Enable();
    }

    private void Start()
    {
        StartCoroutine(GameRunner());
    }

    private void Update()
    {
        UpdateSpeedrunningClock();
        fullTimeTaken += Time.deltaTime;
    }

    private void UpdateSpeedrunningClock()
    {
        if (speedRunningClockText != null)
        {
            string timeTakenStr = "";
            int minTaken = (int)(fullTimeTaken / 60);
            int secondsTaken = (int)(fullTimeTaken % 60);
            int millisecondsTaken = (int)(fullTimeTaken % 1 * 100);
            timeTakenStr += minTaken + ":" + secondsTaken.ToString("D2") + "." + millisecondsTaken.ToString("D2");
            speedRunningClockText.text = timeTakenStr;
        }
    }

    private void SetNumCompleted(int numCompleted)
    {
        if (numCompletedText != null)
        {
            numCompletedText.text = numCompleted + " / " + allInstruments.Count;
        }
    }

    private IEnumerator GameRunner()
    {
        while (instrumentIndex < allInstruments.Count)
        {
            SetNumCompleted(instrumentIndex);
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
            ShowThanks(true);
        }
        curInstrument = newInstrument.Create(instrumentParent);
        curInstrumentInfo = newInstrument;
        yield return StartCoroutine(MoveInstrument(curInstrumentInfo.OffscreenStart(), instrumentPos, instrumentMoveTime, curInstrument));
        ShowThanks(false);
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

    public static Vector2 MousePos()
    {
        return Instance.input.Game.MousePos.ReadValue<Vector2>();
    }

    public static void StartNextSceneTimer()
    {
        Instance.StartCoroutine(Instance.StartNextSceneTimerRoutine());
    }

    private IEnumerator StartNextSceneTimerRoutine()
    {
        yield return new WaitForSeconds(nextSceneTimer);
        SceneSwitcher.LoadScene(nextScene);
    }

    public static void InMinigame(bool inGame)
    {
        if (Instance.hideWhenMinigame != null)
        {
            Instance.hideWhenMinigame.SetActive(!inGame);
        }
    }
}
