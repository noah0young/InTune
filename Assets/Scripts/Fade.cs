using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Image image;
    [SerializeField] private bool fadeInOnStart = false;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private void Start()
    {
        if (fadeInOnStart)
        {
            CallFade();
        }
    }

    public void CallFade()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float timePassed = 0;
        while (timePassed < time)
        {
            image.color = Color.Lerp(startColor, endColor, timePassed / time);
            yield return new WaitForEndOfFrame();
            timePassed += Time.deltaTime;
        }
    }
}
