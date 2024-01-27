using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenSegmentDisplayManager : MonoBehaviour
{
    [SerializeField] private Image[] segments;
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;

    public void SetNumber(int num)
    {
        switch (num)
        {
            case 0:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = onColor;
                segments[3].color = offColor;
                segments[4].color = onColor;
                segments[5].color = onColor;
                segments[6].color = onColor;
                break;
            case 1:
                segments[0].color = offColor;
                segments[1].color = offColor;
                segments[2].color = onColor;
                segments[3].color = offColor;
                segments[4].color = offColor;
                segments[5].color = onColor;
                segments[6].color = offColor;
                break;
            case 2:
                segments[0].color = onColor;
                segments[1].color = offColor;
                segments[2].color = onColor;
                segments[3].color = onColor;
                segments[4].color = onColor;
                segments[5].color = offColor;
                segments[6].color = onColor;
                break;
            case 3:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = offColor;
                segments[3].color = onColor;
                segments[4].color = onColor;
                segments[5].color = offColor;
                segments[6].color = onColor;
                break;
            case 4:
                segments[0].color = offColor;
                segments[1].color = onColor;
                segments[2].color = onColor;
                segments[3].color = onColor;
                segments[4].color = offColor;
                segments[5].color = onColor;
                segments[6].color = offColor;
                break;
            case 5:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = offColor;
                segments[3].color = onColor;
                segments[4].color = offColor;
                segments[5].color = onColor;
                segments[6].color = onColor;
                break;
            case 6:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = offColor;
                segments[3].color = onColor;
                segments[4].color = onColor;
                segments[5].color = onColor;
                segments[6].color = onColor;
                break;
            case 7:
                segments[0].color = onColor;
                segments[1].color = offColor;
                segments[2].color = onColor;
                segments[3].color = offColor;
                segments[4].color = offColor;
                segments[5].color = onColor;
                segments[6].color = offColor;
                break;
            case 8:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = onColor;
                segments[3].color = onColor;
                segments[4].color = onColor;
                segments[5].color = onColor;
                segments[6].color = onColor;
                break;
            case 9:
                segments[0].color = onColor;
                segments[1].color = onColor;
                segments[2].color = onColor;
                segments[3].color = onColor;
                segments[4].color = offColor;
                segments[5].color = onColor;
                segments[6].color = offColor;
                break;
        }
    }
}
