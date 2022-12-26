using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFXController : MonoBehaviour
{
    public static PostFXController Instance;
    
    private Volume fxVolume;

    [SerializeField] private float[] vignetteRange = new float[2];
    [SerializeField] private float[] chromaticAberrationRange = new float[2];
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        fxVolume = gameObject.GetComponent<Volume>();
    }

    private void Start()
    {
        SetVignette(0);
        SetChromaticAberration(0);
    }

    public void SetVignette(float percent)
    {
        if (fxVolume.profile.TryGet<Vignette>(out Vignette vignette))
        {
            var rangeSize = vignetteRange[1] - vignetteRange[0];
            vignette.intensity.value = percent/100 * rangeSize + vignetteRange[0];
        }
    }

    public void SetChromaticAberration(float percent)
    {
        if (fxVolume.profile.TryGet<ChromaticAberration>(out ChromaticAberration chromaticAberration))
        {
            var rangeSize = chromaticAberrationRange[1] - chromaticAberrationRange[0];
            chromaticAberration.intensity.value = percent/100 * rangeSize + chromaticAberrationRange[0];
        }
    }
}
