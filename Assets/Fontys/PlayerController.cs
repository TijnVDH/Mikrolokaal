using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick[] joystick;
    float horMove = 0f;
    float vertMove = 0f;
    float speed = 0.1f;
    int currentJoyStick = 0;

    void Update()
    {
        if (joystick[0].isActiveAndEnabled)
        {
            currentJoyStick = 0;
        }
        else
        {
            currentJoyStick = 1;
        }

        if (joystick[currentJoyStick].Horizontal != 0)
        {
            horMove = joystick[currentJoyStick].Horizontal * speed;
        }
        else
        {
            horMove = 0;
        }

        if (joystick[currentJoyStick].Vertical != 0)
        {
            vertMove = joystick[currentJoyStick].Vertical * speed;
        }
        else
        {
            vertMove = 0;
        }


        Debug.Log($"Horizontal Joystick = {joystick[currentJoyStick].Horizontal.ToString("F2")} Vertical Joystick = {joystick[currentJoyStick].Vertical.ToString("F2")}");
        //Debug.Log($"Horizontal = {horMove} Vertical = {vertMove}");

        transform.Translate(horMove, 0f, vertMove);

    }




}
