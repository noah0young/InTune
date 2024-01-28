using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Serializable]
    private struct AudioWithName
    {
        public AudioClip clip;
        public float volume;
    }

    private static MusicManager Instance;
    private AudioSource source1;
    [SerializeField] private AudioWithName[] clips;
    private static float volume = 1;
    private int curIndex = 0;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        source1 = GetComponent<AudioSource>();
        curIndex = 0;
        source1.clip = clips[curIndex].clip;
        source1.volume = clips[curIndex].volume;
        source1.Play();
    }

    public static void SetSongIndex(int index)
    {
        Instance.curIndex = index;
        Instance.source1.clip = Instance.clips[index].clip;
        Instance.source1.volume = Instance.clips[index].volume * GetVolume();
        Instance.source1.Play();
    }

    public static void EndLoop()
    {
        Instance.source1.loop = false;
    }

    public static void SetVolume(float val)
    {
        volume = val;
        Instance.source1.volume = Instance.clips[Instance.curIndex].volume * GetVolume();
    }

    public static float GetVolume()
    {
        return volume;
    }
}
