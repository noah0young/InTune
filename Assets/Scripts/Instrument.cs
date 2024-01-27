using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField] private List<GameObject> tuningMinigamesPrefabs;
    private List<TuningGame> tuningMinigames = new List<TuningGame>();
    private bool interactable = true;

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
}
