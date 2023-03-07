using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class CrosshaireController : MonoBehaviour
{
    [SerializeField] private GameObject hitmarker;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private TextMeshProUGUI numArrowsDisplay;
    
    // Reference to the UI image component
    private Image image;

    // The start and end colors for the transition
    public Color startColor = Color.white;
    public Color endColor = Color.black;

    // The duration of the transition in seconds
    public float duration = 1f;

    // A flag to indicate if the transition is in progress
    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    private void Start()
    {
        image = crossHair.GetComponent<Image>();
    }

    public void SetNumArrows(int numArrows)
    {
        numArrowsDisplay.text = numArrows.ToString();
    }

    // A coroutine that changes the color of the image using lerp
    private IEnumerator FadeIn()
    {
        // Get the current time
        float startTime = Time.time;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration; // Calculate the fraction of the transition
            image.color = Color.Lerp(startColor, endColor, t); // Lerp the color of the image
            numArrowsDisplay.color = Color.Lerp(startColor, endColor, t); // Lerp the color of the text
            
            yield return null;
        }

        image.color = endColor;
    }
    
    private IEnumerator FadeOut()
    {
        // Get the current time
        float startTime = Time.time;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration; // Calculate the fraction of the transition
            image.color = Color.Lerp(endColor, startColor, t); // Lerp the color of the image
            numArrowsDisplay.color = Color.Lerp(endColor, startColor, t); // Lerp the color of the text
            
            yield return null;
        }

        image.color = startColor;
    }

    public void PlayHitMarker()
    {
        StartCoroutine(HitMarker());
    }

    IEnumerator HitMarker()
    {
        hitmarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitmarker.SetActive(false);
        yield return null;
    }

    public void ShowCrossHair()
    {
        // crossHair.SetActive(true);
        if (fadeOutRoutine != null)
            StopCoroutine(fadeOutRoutine);
        StartCoroutine(FadeIn());
    }

    public void HideCrossHair()
    {
        // crossHair.SetActive(false);
        if (fadeInRoutine != null)
            StopCoroutine(fadeInRoutine);
        StartCoroutine(FadeOut());
    }
}
