using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TextRPGGame : TuningGame
{
    [Serializable]
    private struct AudioClipWithName
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private TMP_Text textboxText;

    [Header("Options")]
    [SerializeField] private Transform optionHolder;
    [SerializeField] private GameObject optionPrefab;

    [Header("Text Params")]
    [SerializeField] private AudioClip defaultTextSound;
    [SerializeField] private List<AudioClipWithName> textSounds;
    [SerializeField] private float timeBetweenChars = 0.1f;
    private AudioSource textSoundPlayer;
    private const string SOUND_ID_START = "[SID";
    private const string TUNED_ID = "[TUNED]";
    private bool fastForwardTextChunk = false;
    private bool tunedAtEndOfDialogue = false;

    [SerializeField] private Dialogue startDialogue;
    private Dialogue curDialogue;

    void StartGame()
    {
        textSoundPlayer = GetComponent<AudioSource>();
        StartCoroutine(RunDialogue());
        StartCoroutine(CheckFastForwardText());
    }

    public override void SetGameOpen(bool open)
    {
        base.SetGameOpen(open);
        if (open)
        {
            StartGame();
        }
        else
        {
            if (curDialogue != null)
            {
                curDialogue.DestroyAllOptionInstances();
            }
        }
    }

    private IEnumerator RunDialogue()
    {
        curDialogue = startDialogue;
        while (curDialogue != null)
        {
            foreach (string t in curDialogue.texts)
            {
                yield return PrintText(t);
                yield return new WaitUntil(() => GameManager.NotClick());
                yield return new WaitUntil(() => GameManager.Click());
            }
            if (curDialogue.HasOptions())
            {
                yield return curDialogue.ChooseOptions(optionHolder, optionPrefab);
            }
            curDialogue = curDialogue.GetNext();
        }
        if (tunedAtEndOfDialogue)
        {
            OnTuned();
        }
        else
        {
            CloseGame();
        }
    }

    private IEnumerator CheckFastForwardText()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.NotClick());
            yield return new WaitUntil(() => GameManager.Click());
            fastForwardTextChunk = true;
        }
    }

    private IEnumerator PrintText(string text)
    {
        fastForwardTextChunk = false;
        textboxText.text = "";
        string soundID = null;
        int index = 0;
        while (index < text.Length)
        {
            if (fastForwardTextChunk)
            {
                if (TunedIDInText(text))
                {
                    tunedAtEndOfDialogue = true;
                }
                textboxText.text = RemoveIDs(text);
                index = text.Length; // Ends the loop
            }
            else
            {
                if (IsTunedAt(text, index))
                {
                    tunedAtEndOfDialogue = true;
                    index = TextIDEndIndex(text, index) + 1;
                }
                else if (IsTextSoundAt(text, index))
                {
                    soundID = ParseTextSoundID(text, index);
                    index = TextIDEndIndex(text, index) + 1;
                }
                else
                {
                    switch (text[index])
                    {
                        case ' ':
                            break;
                        default:
                            PlayTextSound(soundID);
                            break;
                    }
                    textboxText.text += text[index];
                    index++;
                    yield return new WaitForSeconds(timeBetweenChars);
                }
            }
        }
    }

    private bool IsTextSoundAt(string text, int index)
    {
        return IsIDAt(text, index, SOUND_ID_START);
    }

    private bool IsTunedAt(string text, int index)
    {
        return IsIDAt(text, index, TUNED_ID);
    }

    private bool TunedIDInText(string text)
    {
        return text.IndexOf(TUNED_ID) >= 0;
    }

    private bool IsIDAt(string text, int index, string id)
    {
        return text.Length >= index + id.Length
            && text.Substring(index, id.Length) == id;
    }

    private string ParseTextSoundID(string text, int index)
    {
        if (!IsTextSoundAt(text, index))
        {
            throw new Exception("No sound is here");
        }
        index += SOUND_ID_START.Length;
        int indexOfEnd = text.IndexOf(']', index);
        int indexOfEqual = text.IndexOf('=', index);
        if (indexOfEqual < indexOfEnd)
        {
            // Remove equal sign
            index = indexOfEqual + 1;
        }
        string numberWithSpaces = text.Substring(index, indexOfEnd - index);
        return Utility.RemoveSpaces(numberWithSpaces);
    }

    private int TextIDEndIndex(string text, int index)
    {
        return text.IndexOf(']', index);
    }

    private void PlayTextSound(string name)
    {
        AudioClip clip = GetTextSound(name);
        textSoundPlayer.clip = clip;
        textSoundPlayer.Play();
    }

    private AudioClip GetTextSound(string name)
    {
        if (name == null)
        {
            return defaultTextSound;
        }
        for (int i = 0; i < textSounds.Count; i++)
        {
            if (textSounds[i].name == name)
            {
                return textSounds[i].clip;
            }
        }
        throw new Exception("Sound not found");
    }

    private string RemoveIDs(string text)
    {
        StringBuilder builder = new StringBuilder();
        int index = 0;
        while (index < text.Length)
        {
            if (IsTextSoundAt(text, index) || IsTunedAt(text, index))
            {
                index = TextIDEndIndex(text, index) + 1;
            }
            else
            {
                builder.Append(text[index]);
                index++;
            }
        }
        return builder.ToString();
    }
}
