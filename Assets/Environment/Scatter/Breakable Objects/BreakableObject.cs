using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] protected float health;

    public virtual float Health
    {
        private get { return health; }
        set
        {
            // Debug.Log("Damage Taken: " + value);
            
            // Loose health
            health += value;
            
            // If enemy has no more health destroy it
            if (health <= 0)
                Die();
        }
    }
    
    [Serializable]
    public class ItemDrop
    {
        public int dropChance;
        public int dropTries;
        public GameObject drop;
    }
    
    [SerializeField] protected ItemDrop[] itemDrops;
    [SerializeField] protected GameObject deathParticles;

    protected Rigidbody enemyRigidbody;
    
    protected virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }
    
    protected virtual void Die()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);

        for (int i = 0; i < itemDrops.Length; i++)
        {
            for (int j = 0; j < itemDrops[i].dropTries; j++)
            {
                if (Random.Range(1,101) < itemDrops[i].dropChance)
                    Instantiate(itemDrops[i].drop, transform.position, new Quaternion(0,0,0,0));
            }
        }
        
        Destroy(gameObject);
    }
}
