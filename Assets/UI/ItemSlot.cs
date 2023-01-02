using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{ 
    public int inventoryPosition;
    [SerializeField] private Sprite blankSlotImage;
    private InventoryItem inventoryItem;
    private Image _image;
    private bool _full = false;

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
            if (value == null)
            {
                _image.sprite = blankSlotImage;
                _full = false;
                return;
            }
            _image.sprite = inventoryItem.Icon;
            _full = true;
        }
    }
    
    public bool Full
    {
        get { return _full; }
    }

    public void UseItem()
    {
        inventoryItem.UseItem();
        InventoryItem = null;
    }
}
