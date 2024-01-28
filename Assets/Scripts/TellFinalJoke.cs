using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TellFinalJoke : MonoBehaviour
{
    private static TellFinalJoke Instance { get; set; }
    [SerializeField] private float timeBetweenChars = 0.1f;
    [SerializeField] private AudioClip textSound;
    [SerializeField] private string joke = "You can tune a piano, but you can't TUNA fish!";
    [SerializeField] private TMP_Text jokeTextBox;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can only be one Final Joke");
        }
        audioSource = GetComponent<AudioSource>();
    }

    public static void TellJoke()
    {
        Instance.TellJokeInternal();
    }

    private void TellJokeInternal()
    {
        StartCoroutine(JokeCoroutine());
    }

    private IEnumerator JokeCoroutine()
    {
        Debug.Log("Print joke coroutine");
        audioSource.clip = textSound;
        int index = 0;
        jokeTextBox.text = "";
        while (index < joke.Length)
        {
            Debug.Log("Print joke text");
            jokeTextBox.text += joke[index];
            audioSource.Play();
            index += 1;
            yield return new WaitForSeconds(timeBetweenChars);
        }
    }
}
