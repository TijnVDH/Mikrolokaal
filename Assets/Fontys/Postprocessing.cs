using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Postprocessing : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    float old_Value = 0f;

    [SerializeField]
    private Volume vol;

    [Header("Color Adjustments")]
    [SerializeField] private bool CL_Active;
    [SerializeField] private int min_CL_Saturation;
    [SerializeField] private int max_CL_Saturation;

    [Header("Film Grain")]
    [SerializeField] private bool FG_Active;
    [SerializeField] private float min_FG_Intensity;
    [SerializeField] private float max_FG_Intensity;
    [SerializeField] private float offset_FG_Intensity;
    [SerializeField] private float val_FG_Response;

    [Header("Vignette")]
    [SerializeField] private bool V_Active;
    [SerializeField] private float min_V_Intensity;
    [SerializeField] private float max_V_Intensity;
    [SerializeField] private float offset_V_Intensity;
    [SerializeField] private float val_V_Smoothness;
    [SerializeField] private bool val_V_Rounded;

    // Update is called once per frame
    void Update()
    {
        if(slider.value != old_Value)
        {
            if (CL_Active) updateColorAdjustments();
            if (FG_Active) updateFilmGrain();
            if (V_Active) updateVignette();

            old_Value = slider.value;
        }
    }

    void updateColorAdjustments()
    {
        int val = ConvertFrom_Range1_Input_To_Range2_Output(slider.minValue, slider.maxValue, min_CL_Saturation, max_CL_Saturation, slider.value);
        ColorAdjustments _CA;
        if (vol.profile.TryGet<ColorAdjustments>(out _CA))
        {
            _CA.saturation.value = val;
        }
    }

    void updateFilmGrain()
    {
        FilmGrain _FG;
        if (vol.profile.TryGet<FilmGrain>(out _FG))
        {
            _FG.intensity.value = (-1 * (Mathf.Clamp(slider.value,min_FG_Intensity,max_FG_Intensity) - slider.maxValue));
            _FG.response.value = val_FG_Response;
        }
    }

    void updateVignette()
    {
        Vignette _V;
        if (vol.profile.TryGet<Vignette>(out _V))
        {
            _V.intensity.value = (-1 * (Mathf.Clamp(((slider.value - slider.maxValue) + offset_V_Intensity), (-1 * max_V_Intensity), (-1 * min_V_Intensity))));
            _V.smoothness.value = val_V_Smoothness;
            _V.rounded.value = val_V_Rounded;
        }
    }

    private int ConvertFrom_Range1_Input_To_Range2_Output(float _input_range_min,
                                                                float _input_range_max,
                                                                int _output_range_min,
                                                                int _output_range_max,
                                                                float _input_value_tobe_converted)
    {
        int diffOutputRange = Mathf.Abs((_output_range_max - _output_range_min));
        int diffInputRange = Mathf.Abs(((int)(_input_range_max - _input_range_min)));
        int convFactor = (diffOutputRange / diffInputRange);
        return ((int)(_output_range_min + (convFactor * (_input_value_tobe_converted - _input_range_min))));
    }
}
