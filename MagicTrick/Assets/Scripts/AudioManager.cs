using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonPress;
    [SerializeField]
    private AudioClip showtimeButtonPress;
    public static AudioManager Instance;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, 0.05f);
    }

    public void PlayButtonPress()
    {
        audioSource.PlayOneShot(buttonPress, 0.05f);
    }

    public void PlayShowtimeButtonPress()
    {
        audioSource.PlayOneShot(showtimeButtonPress, 0.05f);
    }
}
