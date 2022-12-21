using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [SerializeField] private TextMeshProUGUI arrowDisplay;

    private void Awake()
    {
        // Create coroutine
        if (Instance == null)
            Instance = this;
    }

    public void SetArrow(string arrow)
    {
        Debug.Log(arrow);
        arrowDisplay.text = arrow;
    }
}
