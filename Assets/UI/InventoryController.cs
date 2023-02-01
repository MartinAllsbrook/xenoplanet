using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Image inventoryCursor;

    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private float cursorMoveSpeed;
    
    private Sprite _defaultCursorSprite;
    private int _pickedUpItemPosition;
    private bool _holdingItem;
    
    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].inventoryPosition = i;

        _defaultCursorSprite = inventoryCursor.sprite;
    }

    private void Start()
    {
        InputManager.Instance.select.AddListener(Select);
    }

    /*private void Update()
    {
        var moveDirection = InputManager.Instance.moveDirection;
        inventoryCursor.transform.position += Time.deltaTime * cursorMoveSpeed * new Vector3(moveDirection.x, moveDirection.y, 0);
    }*/

    private void Select()
    {
        Ray ray = new Ray(inventoryCursor.transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
        {
            var tileHit = raycastHit.transform.gameObject.GetComponent<ItemSlot>();
            Debug.Log(tileHit.inventoryPosition);
            if (!_holdingItem && itemSlots[tileHit.inventoryPosition].Full)
            {
                _holdingItem = true;
                _pickedUpItemPosition = tileHit.inventoryPosition;
                inventoryCursor.sprite = itemSlots[tileHit.inventoryPosition].InventoryItem.Icon;
            }
            else if (_holdingItem && !itemSlots[tileHit.inventoryPosition].Full)
            {
                _holdingItem = false;
                itemSlots[tileHit.inventoryPosition].InventoryItem = itemSlots[_pickedUpItemPosition].InventoryItem;
                itemSlots[_pickedUpItemPosition].InventoryItem = null;
                inventoryCursor.sprite = _defaultCursorSprite;
            }
        }
    }

    public void PickUpItem(InventoryItem item)
    {
        // Debug.Log("Picked up item");
        int firstOpenIndex;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].Full)
            {
                itemSlots[i].InventoryItem = item;
                return;
            }
        }
    }
}
