using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource source1;
    [SerializeField] private AudioClip[] clips;

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
        source1.clip = clips[0];
        source1.Play();
    }

    public static void SetSongIndex(int index)
    {
        Instance.source1.clip = Instance.clips[index];
        Instance.source1.Play();
    }
}
