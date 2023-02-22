using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    [SerializeField] protected Rigidbody arrowRigidbody;
    [SerializeField] private float maxDamage;
    [SerializeField] private float maxForce;
    
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
        arrowRigidbody.AddForce(strength * maxForce * transform.forward);
        damage = maxDamage * strength;
    }
    
    private void Update()
    {
        // Always face in the direction the arrow is traveling
        transform.LookAt(transform.position + arrowRigidbody.velocity.normalized);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HUDController.Instance.PlayHitMarker();
            collision.gameObject.GetComponent<Enemy>().Health = -damage;
        }
        if (collision.gameObject.CompareTag("Breakable Environment"))
        {
            collision.gameObject.GetComponent<BreakableObject>().Health = -(damage / 2); // Deal less damage to breakable objects with arrows
        }
        // On collision destroy arrow
        Destroy(gameObject);
    }
}
