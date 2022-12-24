using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [SerializeField] private TextMeshProUGUI arrowDisplay;
    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    // Set arrow display
    public void SetArrow(string arrow)
    {
        // Debug.Log(arrow);
        arrowDisplay.text = arrow;
    }

 
}
