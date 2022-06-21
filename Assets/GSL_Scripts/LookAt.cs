using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform Target;
    public bool lookAtX;
    public bool lookAtY;
    public bool lookAtZ;

    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        transform.LookAt(new Vector3(
            x: lookAtX ? Target.position.x : transform.position.x,
            y: lookAtY ? Target.position.y : transform.position.y,
            z: lookAtZ ? Target.position.z : transform.position.z 
        ));
    }
}
