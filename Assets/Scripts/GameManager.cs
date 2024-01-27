using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Serializable]
    private class InstrumentInfo
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite requestSprite;

        public Instrument Create()
        {
            GameObject instance = Instantiate(prefab);
            return instance.GetComponent<Instrument>();
        }
    }

    public static GameManager Instance { get; private set; }

    [Header("Instruments")]
    [SerializeField] private List<InstrumentInfo> allInstruments;
    private int instrumentIndex = 0;
    private InstrumentInfo curInstrumentInfo = null;
    private Instrument curInstrument;

    [Header("Instrument Movement")]
    [SerializeField] private float instrumentMoveTime = 1;
    [SerializeField] private Transform instrumentStartPos;
    [SerializeField] private Transform instrumentPos;
    [SerializeField] private Transform instrumentEndPos;


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
            StartCoroutine(MoveInstrument(instrumentPos.position, instrumentEndPos.position, instrumentMoveTime, curInstrument));
        }
        curInstrument = newInstrument.Create();
        curInstrumentInfo = newInstrument;
        yield return StartCoroutine(MoveInstrument(instrumentStartPos.position, instrumentPos.position, instrumentMoveTime, curInstrument));
    }

    private IEnumerator MoveInstrument(Vector2 start, Vector2 end, float time, Instrument newInstrument)
    {
        float timePassed = 0;
        while (timePassed < time)
        {
            timePassed += Time.deltaTime;
            newInstrument.transform.position = Vector2.Lerp(start, end, timePassed / time);
            yield return new WaitForEndOfFrame();
        }
    }
}
