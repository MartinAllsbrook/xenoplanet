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
    [SerializeField] private float maxDistance;
    [SerializeField] private float forceMultiplier;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log(itemName);
            if (Inventory.Instance.UpdateItemCount(itemName, 1))
                Destroy(gameObject);
        }    
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            if (!Inventory.Instance.CanStore(itemName))
                return; 

            Vector3 playerPosition = trigger.transform.position - transform.position + Vector3.up * 0.5f;
            Vector3 playerDirection = playerPosition.normalized;
            float playerDistance = playerPosition.magnitude;

            if (playerDistance < maxDistance)
            {
                float force = (maxDistance - playerDistance) / maxDistance * forceMultiplier;
                _rigidbody.AddForce(force * playerDirection);
            }
        }
    }
}
