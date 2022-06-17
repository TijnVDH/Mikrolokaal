using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SFXToggle : NetworkBehaviour
{
    
    bool muted = false;

    [ClientRpc]
    public void ToggleSounds()
    {
        List<AudioSource> sources = new List<AudioSource>();

        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.GetComponent<AudioSource>() != null && g.name != "Music")
            {
                sources.Add(g.GetComponent<AudioSource>());
            }
        }

        if (muted)
        {
            foreach (AudioSource source in sources)
            {
                source.volume = 1;
            }
            muted = false;
        }
        else
        {
            foreach (AudioSource source in sources)
            {
                source.volume = 0;
            }
            muted = true;
        }
    }
}
