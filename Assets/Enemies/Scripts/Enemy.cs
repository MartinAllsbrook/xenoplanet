using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask visible;
    [SerializeField] private GameObject deathParticles;
    // [SerializeField] protected LayerMask player;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // If the enemy collided with an arrow
        if (collision.gameObject.CompareTag("Arrow"))
        {
            // Get arrow script, and arrow damage from script
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            float damage = arrow.Damage;
                
            // Loose health
            health -= damage;
            
            // If enemy has no more health destroy it
            if (health <= 0)
                Die();
        }
    }

    protected virtual void Die()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
