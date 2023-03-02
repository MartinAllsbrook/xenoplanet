using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [SerializeField] private TextMeshProUGUI arrowDisplay;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private CrosshaireController crosshaireController;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI intuitionDisplay;

    private bool inventoryOpen = false;

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        // InputManager.Instance.toggleInventory.AddListener(ToggleInventory);
        // TerrainLoader.Instance.terrainReady.AddListener(DoneLoading);
        inventory.gameObject.SetActive(false);
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

    public void ShowCrossHair()
    {
        crosshaireController.ShowCrossHair();
    }

    public void HideCrossHair()
    {
        crosshaireController.HideCrossHair();
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (inventoryOpen)
        {
            inventory.gameObject.SetActive(false);
            crosshaireController.gameObject.SetActive(true);
            inventoryOpen = false;
        }
        else
        {
            inventory.gameObject.SetActive(true);
            crosshaireController.gameObject.SetActive(false);
            inventoryOpen = true;
        }
    }
    
    // A function that takes an integer and sets the text to that integer
    public void SetIntuition(int number)
    {
        // Convert the integer to a string
        string text = number.ToString();

        // Set the text of the TextMeshPro component
        intuitionDisplay.text = "Intuition: " + text;
    }

    // public void ReadMove(InputAction.CallbackContext context)
    // {
    //     inventoryController.Move(context.ReadValue<Vector2>());
    // }

    public void PickUpItem(string itemName)
    {
        // inventory.PickUpItem(item);
    }

    private void DoneLoading()
    {
        loadingScreen.SetActive(false);
    }
}
