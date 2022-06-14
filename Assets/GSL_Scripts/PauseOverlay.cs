using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PauseOverlay : NetworkBehaviour
{
    public GameObject OverlayContainer;

    [SyncVar]public bool isPaused;
    private bool oldPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        oldPaused = false;
        OverlayContainer.SetActive(false);
    }

    private void Update()
    {
        if(oldPaused != isPaused)
        {
            // set overlay and timescale according to new state
            OverlayContainer.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
            oldPaused = isPaused;
        }
    }

    
    public void RpcTogglePause() {
        // toggle pause state
        isPaused = !isPaused;               
    }

    public override void OnStartClient()
    {
        CmdgetPausedState();
    }

    [Command(requiresAuthority = false)]
    public void CmdgetPausedState()
    {
        RpcSendPausedState(isPaused);
    }

    [ClientRpc]
    public void RpcSendPausedState(bool _state)
    {
        isPaused = _state;
    }
}
