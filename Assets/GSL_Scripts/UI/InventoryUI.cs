using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InventoryUI : MonoBehaviour
{
    public Button DropButton;
    public Character Character;
    public GameObject Joystick;
    [SerializeField] private GameObject Button;

    public void Init(Character character)
    {
        if (character.isServer) 
        {
            Destroy(this.gameObject);
            return;
        } 

        Character = character;
        Button.SetActive(true);
        DropButton.onClick.AddListener(DropInventory);
    }

    private void DropInventory()
    {
        Character.DropInventory();
    }
}
