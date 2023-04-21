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
    
    [SerializeField] private CrosshaireController crosshaireController;
    [SerializeField] private Inventory inventory;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject visibleIndicator;
    
    [Header("Displays")]
    [SerializeField] private TextMeshProUGUI intuitionDisplay;
    [SerializeField] private TextMeshProUGUI arrowDisplay;

    [Header("Health")]
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform shieldBar;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource openInventoryAudio;

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
    
    public void SetShield(float shield)
    {
        // Debug.Log("Health: " + health);
        var width = shield * 5;
        shieldBar.sizeDelta = new Vector2(width, shieldBar.sizeDelta.y);
    }

    public void SetVisibleIndicator(bool visible)
    {
        visibleIndicator.SetActive(visible);
    }

    public void PlayHitMarker()
    {
        crosshaireController.PlayHitMarker(Color.cyan);
    }
    
    public void PlayCriticalMarker()
    {
        crosshaireController.PlayHitMarker(Color.red);
    }

    public void ShowCrossHair()
    {
        crosshaireController.ShowCrossHair();
    }

    public void HideCrossHair()
    {
        crosshaireController.HideCrossHair();
    }

    public void SetNumArrows(int numArrows)
    {
        crosshaireController.SetNumArrows(numArrows);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    
    public void SetPause(bool pause)
    {
        pauseScreen.SetActive(pause);
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (inventoryOpen)
            {
                Debug.Log("Close");
                inventory.gameObject.SetActive(false);
                crosshaireController.gameObject.SetActive(true);
                inventoryOpen = false;
            }
            else
            {
                Debug.Log("Open");
                inventory.gameObject.SetActive(true);
                crosshaireController.gameObject.SetActive(false);
                inventoryOpen = true;
            }
            
            openInventoryAudio.Play();
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

    public void DoneLoading()
    {
        loadingScreen.DoneLoading();
    }
}
