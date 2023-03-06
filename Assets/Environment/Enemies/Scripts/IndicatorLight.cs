using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLight : MonoBehaviour
{
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    
    private bool lightOn = true;
    private bool flashing = false;

    private IEnumerator flashRoutine;

    public bool Flashing
    {
        private get { return flashing; }
        set
        {
            // If flashing changed to true start flashing
            if (!flashing && value)
                StartCoroutine(flashRoutine);
            // If flashing changed to false stop flashing
            else if (flashing && !value)
            {
                StopCoroutine(flashRoutine);
                if (!lightOn)
                    TurnOn();
            }
            // Set flashing
            flashing = value;
        }
    }
    
    private Light pointLight;
    private Renderer materialRenderer;
    private Color emissionColor;
    
    private void Awake()
    {
        pointLight = GetComponent<Light>();
        materialRenderer = GetComponent<Renderer>();
        emissionColor = materialRenderer.material.color;
        flashRoutine = Flash();
    }

    private void Start()
    {
        Flashing = true;
    }

    private IEnumerator Flash()
    {
        while (true)
        {
            ToggleLight();
            yield return new WaitForSeconds(2f);
        }
    }

    private void ToggleLight()
    {
        // Debug.Log("Toggle Light");
        if (lightOn)
            TurnOff();
        else
            TurnOn();    
    }

    // Turn light on
    private void TurnOn()
    {
        pointLight.enabled = true; // Deal with point light
        materialRenderer.material.SetColor("_Color", onColor); // Deal with material color
        materialRenderer.material.SetColor("_EmissionColor", emissionColor); // Deal with material emission
        lightOn = true;
    }
    
    // Turn light off
    private void TurnOff()
    {
        pointLight.enabled = false; // Deal with point light
        materialRenderer.material.SetColor("_Color", offColor); // Deal with material color
        materialRenderer.material.SetColor("_EmissionColor", emissionColor * 0); // Deal with material emission
        lightOn = false;
    }
}
