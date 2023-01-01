using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Image inventoryCursor;

    private void Update()
    {
        var moveDirection = InputManager.Instance.moveDirection;
        inventoryCursor.transform.position += new Vector3(moveDirection.x, moveDirection.y, 0);
    }
}
