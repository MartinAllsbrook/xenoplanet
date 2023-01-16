using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float viewDistance;
    [SerializeField] protected LayerMask visible;
    [SerializeField] private GameObject deathParticles;
    // [SerializeField] protected LayerMask player;

    protected Vector3 targetLocation;
    protected Rigidbody enemyRigidbody;
    
    protected virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        targetLocation = GenerateRandomTarget();
    }

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

    protected virtual Vector3 GenerateRandomTarget()
    {
        return new Vector3(
            transform.position.x + Random.Range(-10f, 10f), 
            transform.position.x + Random.Range(0, 10f),
            transform.position.z + Random.Range(-10f, 10f));
    }
    
    protected virtual void Die()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    /*protected virtual RaycastHit Look()
    {
        Vector3 direction = Player.Instance.transform.position + new Vector3(0, 1, 0) - transform.position;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, range, visible))
        {
            return hit;
        }
    
        return ;
    }*/
}
