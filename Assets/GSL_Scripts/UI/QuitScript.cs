using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class QuitScript : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void CmdQuit()
    {
        Quit();
    }

    [ClientRpc]
    public void Quit()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Application.Quit();
    }
}
