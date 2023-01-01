using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [SerializeField] private TextMeshProUGUI arrowDisplay;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private CrosshaireController crosshaireController;
    [SerializeField] private GameObject inventoryDisplay;
    private bool inventoryOpen = false;

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        InputManager.Instance.toggleInventory.AddListener(ToggleInventory);
        inventoryDisplay.SetActive(false);
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

    public void PlayHitMarker()
    {
        crosshaireController.PlayHitMarker();
    }

    private void ToggleInventory()
    {
        if (inventoryOpen)
        {
            inventoryDisplay.SetActive(false);
            crosshaireController.gameObject.SetActive(true);
            inventoryOpen = false;
        }
        else
        {
            inventoryDisplay.SetActive(true);
            crosshaireController.gameObject.SetActive(false);
            inventoryOpen = true;
        }
    }
}
