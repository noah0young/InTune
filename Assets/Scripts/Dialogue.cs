using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct DialogueOption
{
    public string optionName;
    public Dialogue goTo;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<string> texts;
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
            GameObject optionObj = GameObject.Instantiate(optionPrefab, optionHolder);
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
            GameObject.Destroy(instance);
        }
    }

    public Dialogue GetNext()
    {
        return next;
    }
}