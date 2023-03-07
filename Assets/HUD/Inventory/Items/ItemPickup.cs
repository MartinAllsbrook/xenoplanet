using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private float _floatHeight = 1f;
    private float _rotateSpeed = 1f;
    private float _fallSpeed = 0.1f;
    [SerializeField] private string itemName;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, _rotateSpeed, 0));
        if (!Physics.Raycast(transform.position, Vector3.down, _floatHeight))
            transform.position += Vector3.down * _fallSpeed;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(itemName);
            if (Inventory.Instance.UpdateItemCount(itemName, 1))
                Destroy(gameObject);
        }
    }
}
