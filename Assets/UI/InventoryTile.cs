using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour
{ 
    public int inventoryPosition;
    private InventoryItem inventoryItem;
    private Image _image;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public InventoryItem InventoryItem
    {
        get { return inventoryItem; }
        set
        {
            inventoryItem = value;
            _image.sprite = inventoryItem.Icon;
        }
    }
}
