using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Image light;
    [SerializeField] private Material offmaterial;
    [SerializeField] private Material onmaterial;



    public void SetStatus(bool status)
    {
        if(status)
        {
            light.material = onmaterial;
        } else
        {
            light.material = offmaterial;
        }
    }
}
