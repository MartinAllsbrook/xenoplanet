using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem Item;
    
    private void Start()
    {
        Item.Callback = UseItem;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual void UseItem()
    {
        Debug.Log("Item Used from callback");
    }
}
