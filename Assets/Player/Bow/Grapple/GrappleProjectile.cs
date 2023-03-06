using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody grappleRigidbody;
    [SerializeField] private float velocity;
    
    private void Start()
    {
        grappleRigidbody.velocity = velocity * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Set grapple instance as hooked
        Grapple.Instance.Hook(transform.position);

        // Destroy gameObject
        Destroy(gameObject);
    }
}
