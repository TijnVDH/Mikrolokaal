using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class tchange : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;
    [SerializeField]
    private Volume vol;

    public void onValueChange(float val)
    {
        tmp.text = val.ToString();
        ColorAdjustments _CA;
        if(vol.profile.TryGet<ColorAdjustments>(out _CA))
        {
            _CA.saturation.value = val - 100;
        }
    }
}
