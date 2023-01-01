using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryDisplay;
    private bool inventoryOpen = false;
    
    void Start()
    {
        InputManager.Instance.toggleInventory.AddListener(ToggleInventory);
        inventoryDisplay.SetActive(false);
    }

    private void ToggleInventory()
    {
        if (inventoryOpen)
        {
            inventoryDisplay.SetActive(false);
            inventoryOpen = false;
        }
        else
        {
            inventoryDisplay.SetActive(true);
            inventoryOpen = true;
        }
    }  
}
