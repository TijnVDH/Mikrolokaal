using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    AudioSource musicSource;
    bool gamePaused;
    bool wasPlaying;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        gamePaused = false;
        wasPlaying = false;
    }

    public void StartMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ToggleMusic()
    {
        if(musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }

    public void GamePause()
    {
        if(gamePaused)
        {
            if(wasPlaying)
            {
                musicSource.UnPause();
            }
            gamePaused = false;
        }
        else
        {
            if(musicSource.isPlaying)
            {
                musicSource.Pause();
                wasPlaying = true;
            }
            else
            {
                wasPlaying = false;
            }
            gamePaused = true;
        }
    }
}
