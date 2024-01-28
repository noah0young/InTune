using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpinGame : TuningGame
{
    [SerializeField] private TMP_Text numberToGoText;
    [SerializeField] private Transform screw;
    [SerializeField] private Transform toolHolder;
    [SerializeField] private float lapAt1 = 0; // degrees
    [SerializeField] private float lapAt2 = 180; // degrees
    [SerializeField] private float range = 5; // degrees
    [SerializeField] private int halfRotationsNeeded = 6;
    private float toolHolderRotation;
    private int halfLapsPerformed;

    private void Update()
    {
        CheckTuned();
        UpdateRotation();
        UpdateUI();
    }

    private void CheckTuned()
    {
        if (halfLapsPerformed >= halfRotationsNeeded)
        {
            OnTuned();
        }
    }

    private void UpdateRotation()
    {
        Vector2 mousePos = GameManager.MousePos();
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        float angle = Vector2.Angle(mousePos, screw.position);
        toolHolderRotation = angle;
        toolHolder.eulerAngles = new Vector3(0, 0, toolHolderRotation);
        float curLapAt = halfLapsPerformed % 2 == 0 ? lapAt1 : lapAt2;
        if (toolHolderRotation > curLapAt - range && toolHolderRotation < curLapAt + range)
        {
            Debug.Log("rotation at " + toolHolderRotation + " with curLapAt " + curLapAt);
            halfLapsPerformed += 1;
        }
    }

    private void UpdateUI()
    {
        numberToGoText.text = "" + (halfRotationsNeeded - halfLapsPerformed);
    }
}
