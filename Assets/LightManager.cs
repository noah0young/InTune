using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Image lightIcon;
    [SerializeField] private Material offmaterial;
    [SerializeField] private Material onmaterial;



    public void SetStatus(bool status)
    {
        if(status)
        {
            lightIcon.material = onmaterial;
        } else
        {
            lightIcon.material = offmaterial;
        }
    }
}
