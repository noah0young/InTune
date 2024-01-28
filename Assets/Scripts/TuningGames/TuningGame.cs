using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TuningGame : MonoBehaviour
{
    private Instrument tunesFor = null;
    private int tuningIndex = -1;
    private bool tuned = false;

    public void SetInstrument(Instrument tunesFor, int tuningIndex)
    {
        this.tunesFor = tunesFor;
        this.tuningIndex = tuningIndex;
    }

    protected void OnTuned()
    {
        tuned = true;
        tunesFor.SetTuned(tuningIndex);
        tunesFor.CloseGame(tuningIndex);
    }

    public bool IsTuned()
    {
        return tuned;
    }

    public virtual void SetGameOpen(bool open)
    {
        GameManager.InMinigame(open);
        gameObject.SetActive(open);
    }

    public void CloseGame()
    {
        tunesFor.CloseGame(tuningIndex);
    }
}
