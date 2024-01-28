using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberScreen : MonoBehaviour
{
    [SerializeField] private SevenSegmentDisplayManager thousands;
    [SerializeField] private SevenSegmentDisplayManager hundreds;
    [SerializeField] private SevenSegmentDisplayManager tens;
    [SerializeField] private SevenSegmentDisplayManager ones;
    [SerializeField] private LightManager light;
    [SerializeField] private int currentNumber;
    [SerializeField] private int lowerBound;
    [SerializeField] private int upperBound;


    private void Update()
    {
        UpdateNumber();
    }

    private void UpdateNumber()
    {
        string numbero = currentNumber.ToString();
        while(numbero.Length < 4)
        {
            numbero = "0" + numbero;
        }

        thousands.SetNumber(int.Parse(numbero[0].ToString()));
        hundreds.SetNumber(int.Parse(numbero[1].ToString()));
        tens.SetNumber(int.Parse(numbero[2].ToString()));
        ones.SetNumber(int.Parse(numbero[3].ToString()));

        if (lowerBound <= currentNumber && currentNumber >=upperBound)
        {
            light.SetStatus(true);
        }
        else
        {
            light.SetStatus(false);
        }
    }

    public void increment(int amount = 10)
    {
        currentNumber += amount;
    }

    public void decrement(int amount = 10)
    {
        currentNumber -= amount;
        if(currentNumber < 0)
        {
            currentNumber = 0;
        }
    }

    public int GetNumber()
    {
        return currentNumber;
    }
    
    public void SetUpperBound(int bound)
    {
        upperBound = bound;
    }

    public void SetLowerBound(int bound)
    {
        lowerBound = bound;
    }

}
