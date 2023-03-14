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
    [SerializeField] private float criticalMultiplier;
    [SerializeField] private float maxForce;
    private const float ArrowUp = 0.005f;

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
        arrowRigidbody.AddForce(strength * maxForce * (transform.forward + transform.up * ArrowUp));
        damage = maxDamage * strength;
    }
    
    private void Update()
    {
        // Always face in the direction the arrow is traveling
        transform.LookAt(transform.position + arrowRigidbody.velocity.normalized);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            CheckEnemyHit(collision);
        
        if (collision.gameObject.CompareTag("Breakable Environment"))
            collision.gameObject.GetComponent<BreakableObject>().Health = -(damage / 2); // Deal less damage to breakable objects with arrows
        
        Destroy(gameObject);
    }

    private void CheckEnemyHit(Collision collision)
    { 
        // Check if the collider we hit was the enemies critical hit collider
        foreach (var contactPoint in collision.contacts)
        {
            Debug.Log(contactPoint.otherCollider.tag);
            if (contactPoint.otherCollider.CompareTag("EnemyCritical"))
            {
                HUDController.Instance.PlayCriticalMarker();
                collision.gameObject.GetComponent<Enemy>().Health = -damage * criticalMultiplier;
                return;
            }
        }
        HUDController.Instance.PlayHitMarker();
        collision.gameObject.GetComponent<Enemy>().Health = -damage;
    }
}
