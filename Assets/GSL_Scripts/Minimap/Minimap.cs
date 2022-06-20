using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    GameObject localPlayer;

    private void LateUpdate()
    {
        //followPlayer();

        // refresh
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.GetComponent<Character>() != null)
            {
                if (obj.GetComponent<Character>().CharacterType == CharacterType.Player && obj != localPlayer)
                {
                    obj.GetComponent<Character>().minimapIcon.SetActive(false);
                }
            }
        }
    }

    public void SetPlayer(GameObject player)
    {
        localPlayer = player;
    }

    void followPlayer()
    {
        if (localPlayer == null) return;
        Vector3 newPosition = localPlayer.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
