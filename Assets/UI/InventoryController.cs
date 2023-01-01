using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Image inventoryCursor;

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
        }
    }

}
