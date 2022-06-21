using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapReset : MonoBehaviour
{
    GameObject minimap;

    void Start()
    {
        minimap = GameObject.Find("MinimapWindow");
        if(gameObject.GetComponent<Character>() != null && minimap != null)
        {
            if(gameObject.GetComponent<Character>().isServer)
            {
                minimap.SetActive(false);
            }
        }
    }
}
