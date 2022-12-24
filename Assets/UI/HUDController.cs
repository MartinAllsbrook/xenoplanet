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
    [SerializeField] private RectTransform healthBar;
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

    // Set health bar width
    public void SetHealth(float health)
    {
        // Debug.Log("Health: " + health);
        var width = health * 5;
        healthBar.sizeDelta = new Vector2(width, healthBar.sizeDelta.y);
    }
}
