using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem Item;
    [SerializeField] private LayerMask canLandOn;
    [SerializeField] private float floatHeight;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float fallSpeed;
    
    private void Start()
    {
        Item.Callback = UseItem;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, rotateSpeed, 0));
        if (!Physics.Raycast(transform.position, Vector3.down, floatHeight))
            transform.position += Vector3.down * fallSpeed;    
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
        Debug.Log("No use item method defined");
    }
}
