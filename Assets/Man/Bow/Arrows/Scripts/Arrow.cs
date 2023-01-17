using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    [SerializeField] protected Rigidbody arrowRigidbody;
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseForce;
    
    [SerializeField] private string arrowName;
    public string ArrowName
    {
        get { return this.arrowName; }
        private set {}
    }
    
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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HUDController.Instance.PlayHitMarker();
            collision.gameObject.GetComponent<Enemy>().Health = -damage;
        }
        
        // On collision destroy arrow
        Destroy(gameObject);
    }
}
