using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingTimeDisplay;
    
    private float _loadTime = 0;
    private void Update()
    {
        _loadTime += Time.deltaTime;
        loadingTimeDisplay.text = "" + "Loading Time: " + _loadTime.ToString("F1") + "s";
    }
}
