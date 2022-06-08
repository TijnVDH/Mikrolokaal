using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickChanger : MonoBehaviour
{
    public GameObject fixedJoystick;
    public GameObject floatingJoystick;
    bool isFixed = true;


    public void ChangeJoyStick()
    {
        if (isFixed)
        {
            floatingJoystick.SetActive(true);
            fixedJoystick.SetActive(false);
            isFixed = !isFixed;
        }
        else
        {
            floatingJoystick.SetActive(false);
            fixedJoystick.SetActive(true);
            isFixed = !isFixed;
        }
    }
}
