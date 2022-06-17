using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PauseOverlay : NetworkBehaviour
{
    public GameObject OverlayContainer;

    private bool isPaused;

    public GameObject musicButtonHolder;
    public GameObject soundButtonHolder;

    Button musicButton;
    Button soundButton;

    private void Awake()
    {
        musicButton = musicButtonHolder.GetComponent<Button>();
        soundButton = soundButtonHolder.GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        OverlayContainer.SetActive(false);
    }

    [ClientRpc] public void RpcTogglePause()
    {
        // toggle pause state
        isPaused = !isPaused;
        toggleSoundButtons();

        // set overlay and timescale according to new state
        OverlayContainer.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    void toggleSoundButtons()
    {
        if(isPaused)
        {
            musicButton.interactable = false;
            soundButton.interactable = false;
        }
        else
        {
            musicButton.interactable = true;
            soundButton.interactable = true;
        }
    }
}
