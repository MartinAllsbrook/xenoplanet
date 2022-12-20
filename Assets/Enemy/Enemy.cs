using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the enemy collides with an arrow
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            float damage = arrow.Damage;
                
            // Loose health
            health -= damage;
            Debug.Log(health);
            
            // If enemy has no more health destroy it
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
