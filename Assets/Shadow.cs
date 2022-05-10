using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public LookAt RotationController;
    // Start is called before the first frame update
    void Start()
    {
        RotationController.Target = FindObjectOfType<Soup>().transform;
    }
}
