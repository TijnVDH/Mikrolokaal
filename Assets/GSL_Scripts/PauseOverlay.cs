using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PauseOverlay : NetworkBehaviour
{
    public GameObject OverlayContainer;

    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        OverlayContainer.SetActive(false);
    }

    [ClientRpc] public void RpcTogglePause() {
        // toggle pause state
        isPaused = !isPaused;

        // set overlay and timescale according to new state
        OverlayContainer.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;                
    }
}
