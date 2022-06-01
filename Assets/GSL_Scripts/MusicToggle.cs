using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    AudioSource musicSource;
    bool playing;
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        playing = false;
    }

    public void StartMusic()
    {
        musicSource.Play();
        playing = true;
    }

    public void StopMusic()
    {
        musicSource.Stop();
        playing = false;
    }

    public void ToggleMusic()
    {
        if(playing)
        {
            musicSource.mute = true;
            playing = false;
        }
        else
        {
            musicSource.mute = false;
            playing = true;
        }
    }
}
