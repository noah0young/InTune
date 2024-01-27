using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TextRPGGame : TuningGame
{
    [Serializable]
    private struct DialogueOption
    {
        public string optionName;
        public Dialogue goTo;
    }

    [Serializable]
    private class Dialogue
    {
        public string text;
        public List<DialogueOption> options;
        private List<GameObject> optionInstances = new List<GameObject>();
        private Dialogue next = null;

        public bool HasOptions()
        {
            return options.Count > 0;
        }

        public IEnumerator ChooseOptions(Transform optionHolder, GameObject optionPrefab)
        {
            optionInstances.Clear();
            bool optionChosen = false;
            for (int i = 0; i < options.Count; i++)
            {
                DialogueOption option = options[i];
                GameObject optionObj = Instantiate(optionPrefab, optionHolder);
                optionInstances.Add(optionObj);
                optionObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    optionChosen = true;
                    next = option.goTo;
                });
                optionObj.GetComponentInChildren<TMP_Text>().text = option.optionName;
            }
            yield return new WaitUntil(() => optionChosen);
            DestroyAllOptionInstances();
        }

        private void DestroyAllOptionInstances()
        {
            foreach (GameObject instance in optionInstances)
            {
                Destroy(instance);
            }
        }

        public Dialogue GetNext()
        {
            return next;
        }
    }

    [SerializeField] private TMP_Text text;

    [Header("Options")]
    [SerializeField] private Transform optionHolder;
    [SerializeField] private GameObject optionPrefab;

    [SerializeField] private List<Dialogue> dialogues;

    void Start()
    {
        StartCoroutine(RunDialogue());
    }

    private IEnumerator RunDialogue()
    {
        Dialogue curDialogue = dialogues[0];
        while (curDialogue != null && curDialogue.text != "")
        {
            text.text = curDialogue.text;
            if (!curDialogue.HasOptions())
            {
                yield return new WaitUntil(() => GameManager.Click());
            }
            else
            {
                yield return curDialogue.ChooseOptions(optionHolder, optionPrefab);
            }
            curDialogue = curDialogue.GetNext();
        }
        OnTuned();
    }
}
