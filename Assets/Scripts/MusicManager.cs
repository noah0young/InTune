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
        source1.clip = clips[0].clip;
        source1.volume = clips[0].volume;
        source1.Play();
    }

    public static void SetSongIndex(int index)
    {
        Instance.source1.clip = Instance.clips[index].clip;
        Instance.source1.volume = Instance.clips[index].volume;
        Instance.source1.Play();
    }

    public static void EndLoop()
    {
        Instance.source1.loop = false;
    }
}
