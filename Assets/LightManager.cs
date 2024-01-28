using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer light;
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
