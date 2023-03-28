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
    [SerializeField] private GameObject crossHairCenter;
    [SerializeField] private GameObject crossHairLeft;
    [SerializeField] private GameObject crossHairRight;
    [SerializeField] private TextMeshProUGUI numArrowsDisplay;
    [SerializeField] private float minCrossHairWidth;
    [SerializeField] private float maxCrossHairWidth;

    // The start and end colors for the transition
    public Color startColor = Color.white;
    public Color endColor = Color.black;

    // The duration of the transition in seconds
    public float duration = 1f;

    // A flag to indicate if the transition is in progress
    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    private Image _crossHairCenterImage;
    private Image _crossHairLeftImage;
    private Image _crossHairRightImage;
    
    private RectTransform _crossHairLeftRectTransform;
    private RectTransform _crossHairRightRectTransform;

    private AudioSource _hitMarkerSound;

    private void Start()
    {
        _crossHairCenterImage = crossHairCenter.GetComponent<Image>();
        _crossHairLeftImage = crossHairLeft.GetComponent<Image>();
        _crossHairRightImage = crossHairRight.GetComponent<Image>();
        
        _crossHairLeftRectTransform = crossHairLeft.GetComponent<RectTransform>();
        _crossHairRightRectTransform = crossHairRight.GetComponent<RectTransform>();

        _hitMarkerSound = GetComponent<AudioSource>();
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
            SetColor(Color.Lerp(startColor, endColor, t));
            yield return null;
        }

        SetColor(endColor);
    }

    private IEnumerator FadeOut()
    {
        // Get the current time
        float startTime = Time.time;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration; // Calculate the fraction of the transition
            SetColor(Color.Lerp(endColor, startColor, t)); // Lerp the color of the text
            yield return null;
        }

        SetColor(startColor);
    }

    private void SetColor(Color color)
    {
        _crossHairCenterImage.color = color;
        _crossHairLeftImage.color = color;
        _crossHairRightImage.color = color;
        numArrowsDisplay.color = color;
    }

    public void PlayHitMarker(Color color)
    {
        StartCoroutine(HitMarker(color));
        _hitMarkerSound.Play();
    }

    IEnumerator HitMarker(Color color)
    {
        hitmarker.SetActive(true);
        hitmarker.GetComponent<Image>().color = color;
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

    public void SetCrossHairWidth(float percent)
    {
        float width = maxCrossHairWidth - minCrossHairWidth;
        float position = minCrossHairWidth + width * (1 - percent);
        // Debug.Log("Percent: " + percent);
        // Debug.Log("Position: " + position);
        _crossHairLeftRectTransform.anchoredPosition = new Vector2(-position, _crossHairLeftRectTransform.anchoredPosition.y);
        _crossHairRightRectTransform.anchoredPosition = new Vector2(position, _crossHairLeftRectTransform.anchoredPosition.y);

    }
}
