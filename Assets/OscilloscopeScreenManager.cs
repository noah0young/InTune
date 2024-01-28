using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilloscopeScreenManager : TuningGame
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject over;
    [SerializeField] private GameObject under;
    [SerializeField] private GameObject critical;
    [SerializeField] private GameObject disaster;
    [SerializeField] private int PLowerBound;
    [SerializeField] private int PUpperBound;
    [SerializeField] private int ILowerBound;
    [SerializeField] private int IUpperBound;
    [SerializeField] private int DLowerBound;
    [SerializeField] private int DUpperBound;
    [SerializeField] private NumberScreen P;
    [SerializeField] private NumberScreen I;
    [SerializeField] private NumberScreen D;
    [SerializeField] private string status;

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.worldCamera = Camera.main;

        P.SetUpperBound(PUpperBound);
        P.SetLowerBound(PLowerBound);

        I.SetUpperBound(IUpperBound);
        I.SetLowerBound(ILowerBound);

        D.SetUpperBound(DUpperBound);
        D.SetLowerBound(DLowerBound);
    }

    // Update is called once per frame
    void Update()
    {
        int pvalue = P.GetNumber();
        int ivalue = I.GetNumber();
        int dvalue = D.GetNumber();
        int over = 0;
        int under = 0;
        int correct = 0;
        if(PLowerBound <= pvalue)
        {
            if(pvalue <= PUpperBound)
            {
                correct += 1;
            }else
            {
                over += 1;
            }
        }
        else
        {
            under += 1;
        }
        if (ILowerBound <= ivalue)
        {
            if (ivalue <= IUpperBound)
            {
                correct += 1;
            }
            else
            {
                over += 1;
            }
        }
        else
        {
            under += 1;
        }
        if (DLowerBound <= dvalue)
        {
            if (dvalue <= DUpperBound)
            {
                correct += 1;
            }
            else
            {
                over += 1;
            }
        }
        else
        {
            under += 1;
        }
        if (correct == 3)
        {
            status = "critical";
            OnTuned();
        } else if(over == 3 || under == 3)
        {
            status = "disaster";
        } else if(over > under)
        {
            status = "over";
        }
        else
        {
            status = "under";
        }
        setImage(status);

    }

    void setImage(string state)
    {
        switch(state)
        {
            case "over":
                over.SetActive(true);
                under.SetActive(false);
                critical.SetActive(false);
                disaster.SetActive(false);
                break;
            case "under":
                over.SetActive(false);
                under.SetActive(true);
                critical.SetActive(false);
                disaster.SetActive(false);
                break;
            case "critical":
                over.SetActive(false);
                under.SetActive(false);
                critical.SetActive(true);
                disaster.SetActive(false);
                break;
            case "disaster":
                over.SetActive(false);
                under.SetActive(false);
                critical.SetActive(false);
                disaster.SetActive(true);
                break;
        }
    }
}
