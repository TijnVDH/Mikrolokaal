using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCircle : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 0, -1);
    }
}
