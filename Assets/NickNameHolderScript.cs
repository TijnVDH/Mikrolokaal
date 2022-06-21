using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickNameHolderScript : MonoBehaviour
{
    private string _name = "";
    public string NickName { get { return _name; } set { _name = value; } }

    [SerializeField] TMP_Dropdown DropDown;

    public void ChangeName()
    {
        NickName = DropDown.options[DropDown.value].text ?? "";
    }
}
