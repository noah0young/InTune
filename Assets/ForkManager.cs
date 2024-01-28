using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkManager : MonoBehaviour
{
    [SerializeField] private GameObject forks;
        
    // Start is called before the first frame update
    private void Start()
    {
        forks.SetActive(false);
    }

}
