using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenSegmentDisplayManager : MonoBehaviour
{
    [SerializeField] private Image[] segments;
    [SerializeField] private Material offmaterial;
    [SerializeField] private Material onmaterial;
    [SerializeField] private int numDebug = 0; //DEBUG

    [ContextMenu("CycleNumberDebug")]
    public void CycleNumberDebug()
    {
        SetNumber(numDebug);
        numDebug += 1;
        numDebug %= 10;
    }

    public void SetNumber(int num)
    {
        numDebug = num;
        num %= 10;
        switch (num)
        {
            case 0:
                segments[0].material = onmaterial;
                segments[1].material = onmaterial;
                segments[2].material = onmaterial;
                segments[3].material = offmaterial;
                segments[4].material = onmaterial;
                segments[5].material = onmaterial;
                segments[6].material = onmaterial;
                break;
            case 1:
                segments[0].material = offmaterial;
                segments[1].material = offmaterial;
                segments[2].material = onmaterial;
                segments[3].material = offmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = offmaterial;
                break;
            case 2:
                segments[0].material = onmaterial;
                segments[1].material = offmaterial;
                segments[2].material = onmaterial;
                segments[3].material = onmaterial;
                segments[4].material = onmaterial;
                segments[5].material = offmaterial;
                segments[6].material = onmaterial;
                break;
            case 3:
                segments[0].material = onmaterial;
                segments[1].material = offmaterial;
                segments[2].material = onmaterial;
                segments[3].material = onmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = onmaterial;
                break;
            case 4:
                segments[0].material = offmaterial;
                segments[1].material = onmaterial;
                segments[2].material = onmaterial;
                segments[3].material = onmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = offmaterial;
                break;
            case 5:
                segments[0].material = onmaterial;
                segments[1].material = onmaterial;
                segments[2].material = offmaterial;
                segments[3].material = onmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = onmaterial;
                break;
            case 6:
                segments[0].material = onmaterial;
                segments[1].material = onmaterial;
                segments[2].material = offmaterial;
                segments[3].material = onmaterial;
                segments[4].material = onmaterial;
                segments[5].material = onmaterial;
                segments[6].material = onmaterial;
                break;
            case 7:
                segments[0].material = onmaterial;
                segments[1].material = offmaterial;
                segments[2].material = onmaterial;
                segments[3].material = offmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = offmaterial;
                break;
            case 8:
                segments[0].material = onmaterial;
                segments[1].material = onmaterial;
                segments[2].material = onmaterial;
                segments[3].material = onmaterial;
                segments[4].material = onmaterial;
                segments[5].material = onmaterial;
                segments[6].material = onmaterial;
                break;
            case 9:
                segments[0].material = onmaterial;
                segments[1].material = onmaterial;
                segments[2].material = onmaterial;
                segments[3].material = onmaterial;
                segments[4].material = offmaterial;
                segments[5].material = onmaterial;
                segments[6].material = offmaterial;
                break;
        }
    }
}
