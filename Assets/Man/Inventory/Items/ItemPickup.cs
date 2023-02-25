using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // public InventoryItem Item;
    [SerializeField] private LayerMask canLandOn;
    [SerializeField] private float floatHeight;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private string itemName;
    
    private void Start()
    {
        // Item.Callback = UseItem;
    }

    /*public string GetName()
    {
        return itemName;
    }*/

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, rotateSpeed, 0));
        if (!Physics.Raycast(transform.position, Vector3.down, floatHeight))
            transform.position += Vector3.down * fallSpeed;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(itemName);
            if (Inventory.Instance.PickUpItem(itemName))
                Destroy(gameObject);
        }
    }

    // Code from when this was not a trigger
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }*/
    
    // protected virtual void UseItem()
    // {
    //     Debug.Log("No use item method defined");
    // }
}
