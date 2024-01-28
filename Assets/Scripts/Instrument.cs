using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instrument : MonoBehaviour
{
    [SerializeField] private bool isFish = false;
    [SerializeField] private List<GameObject> tuningMinigamesPrefabs;
    private List<TuningGame> tuningMinigames = new List<TuningGame>();
    private bool interactable = true;

    [Header("Tuning Target UI")]
    [SerializeField] private List<Button> targetButtons;
    [SerializeField] private Material checkTargetMat;
    [SerializeField] private Material questionTargetMat;

    private void Start()
    {
        for (int i = 0; i < tuningMinigamesPrefabs.Count; i++)
        {
            GameObject minigameObj = Instantiate(tuningMinigamesPrefabs[i]);
            TuningGame minigame = minigameObj.GetComponent<TuningGame>();
            tuningMinigames.Add(minigame);
            minigame.SetInstrument(this, i);
            minigame.SetGameOpen(false);
        }

        for (int i = 0; i < targetButtons.Count; i++)
        {
            int buttonIndex = i;
            targetButtons[i].onClick.AddListener(() => OpenMinigame(buttonIndex));
        }
    }

    public bool IsTuned()
    {
        foreach (TuningGame tuningGame in tuningMinigames)
        {
            if (!tuningGame.IsTuned())
            {
                return false;
            }
        }
        return true;
    }

    public void OpenMinigame(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex > tuningMinigames.Count)
        {
            throw new System.Exception("Minigame out of bounds");
        }
        if (!tuningMinigames[buttonIndex].IsTuned() && interactable)
        {
            interactable = false;
            tuningMinigames[buttonIndex].SetGameOpen(true);
        }
    }

    public void CloseGame(int buttonIndex)
    {
        if (tuningMinigames.Count > buttonIndex)
        {
            interactable = true;
            tuningMinigames[buttonIndex].SetGameOpen(false);
            if (isFish)
            {
                SetButtonToQuestionMark(buttonIndex);
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        DestroyAllMinigames();
    }

    private void DestroyAllMinigames()
    {
        foreach (TuningGame game in tuningMinigames)
        {
            if (game != null)
            {
                Destroy(game.gameObject);
            }
        }
    }

    public void SetTuned(int buttonIndex)
    {
        if (tuningMinigames[buttonIndex].IsTuned())
        {
            targetButtons[buttonIndex].GetComponent<Image>().material = checkTargetMat;
        }
    }

    private void SetButtonToQuestionMark(int buttonIndex)
    {
        targetButtons[buttonIndex].GetComponent<Image>().material = questionTargetMat;
    }
}
