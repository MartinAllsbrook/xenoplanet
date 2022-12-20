using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody arrowRigidbody;
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseForce;
    
    // Arrow damage var and prop to be accessed by target
    private float damage = 0;
    public float Damage
    {
        get { return damage; }
        private set { }
    }
    
    // Fire: Add fore and set damage of arrow
    public void Fire(float strength)
    {
        arrowRigidbody.AddForce(strength * baseForce * transform.forward);
        damage = baseDamage * strength;
    }
    
    private void Update()
    {
        // Always face in the direction the arrow is traveling
        transform.LookAt(transform.position + arrowRigidbody.velocity.normalized);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // On collision destroy arrow
        Destroy(gameObject);
    }
}
