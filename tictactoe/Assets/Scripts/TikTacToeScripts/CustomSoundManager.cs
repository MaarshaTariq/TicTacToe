using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSoundManager : MonoBehaviour
{
    public AudioClip[] SoundClips;
    AudioSource AudioPlayer;

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }

    void PlaySoundClip(int i)
    {
        AudioPlayer.PlayOneShot(SoundClips [i]);
    }

    public void PlayMarkSound()
    {
        PlaySoundClip(0);
    }

    public void PlayCompletedSound()
    {
        PlaySoundClip(1);
    }
}
