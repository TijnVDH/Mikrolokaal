using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FXToggle : MonoBehaviour
{
    List<AudioSource> sources = new List<AudioSource>();
    bool muted = false;

    void Start()
    {
        // get all audio sources
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if(g.GetComponent<AudioSource>() != null && g.name != "Music")
            {
                sources.Add(g.GetComponent<AudioSource>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // refresh list
        sources.Clear();
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.GetComponent<AudioSource>() != null && g.name != "Music")
            {
                sources.Add(g.GetComponent<AudioSource>());
            }
        }
    }

    public void ToggleSounds()
    {
        if(muted)
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
