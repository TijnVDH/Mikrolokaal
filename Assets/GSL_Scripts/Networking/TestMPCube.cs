using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMPCube : MonoBehaviour
{
    public float RotateSpeedMin = 1;
    public float RoateSpeedMax = 1000;

    private float rotateSpeed;

    private void Start()
    {
        rotateSpeed = Random.Range(RotateSpeedMin, RoateSpeedMax);
        transform.DORotate(new Vector3(100, 100, 100), rotateSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetSpeedBased(true).SetEase(Ease.Linear);
    }
}
