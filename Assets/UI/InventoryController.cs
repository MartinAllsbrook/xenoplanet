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

    [SerializeField] private InventoryTile[] _inventoryTiles;

    private Sprite _defaultCursorSprite;
    private int _pickedUpItemPosition;
    private bool _holdingItem;
    
    private void Awake()
    {
        for (int i = 0; i < _inventoryTiles.Length; i++)
            _inventoryTiles[i].inventoryPosition = i;

        _defaultCursorSprite = inventoryCursor.sprite;
    }

    private void Start()
    {
        InputManager.Instance.select.AddListener(Select);
    }

    private void Update()
    {
        var moveDirection = InputManager.Instance.moveDirection;
        inventoryCursor.transform.position += new Vector3(moveDirection.x, moveDirection.y, 0);
    }

    private void Select()
    {
        Ray ray = new Ray(inventoryCursor.transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
        {
            var tileHit = raycastHit.transform.gameObject.GetComponent<InventoryTile>();
            Debug.Log(tileHit.inventoryPosition);
            if (!_holdingItem && _inventoryTiles[tileHit.inventoryPosition].Full)
            {
                _holdingItem = true;
                _pickedUpItemPosition = tileHit.inventoryPosition;
                inventoryCursor.sprite = _inventoryTiles[tileHit.inventoryPosition].InventoryItem.Icon;
            }
            else if (_holdingItem && !_inventoryTiles[tileHit.inventoryPosition].Full)
            {
                _holdingItem = false;
                _inventoryTiles[tileHit.inventoryPosition].InventoryItem = _inventoryTiles[_pickedUpItemPosition].InventoryItem;
                _inventoryTiles[_pickedUpItemPosition].InventoryItem = null;
                inventoryCursor.sprite = _defaultCursorSprite;
            }
        }
    }

    public void PickUpItem(InventoryItem item)
    {
        Debug.Log("Picked up item");
        int firstOpenIndex;
        for (int i = 0; i < _inventoryTiles.Length; i++)
        {
            if (!_inventoryTiles[i].Full)
            {
                _inventoryTiles[i].InventoryItem = item;
                return;
            }
        }
    }
}
