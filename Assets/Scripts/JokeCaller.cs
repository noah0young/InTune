using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeCaller : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Tell Joke");
        TellFinalJoke.TellJoke();
    }
}
