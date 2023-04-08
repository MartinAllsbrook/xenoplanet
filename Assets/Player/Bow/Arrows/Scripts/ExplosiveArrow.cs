using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveArrow : Arrow
{
    [SerializeField] protected GameObject explodeParticles;
    [SerializeField] protected LayerMask enemies;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected float explosionFalloff;
    [SerializeField] protected float maxExplosionDamage;

    protected override void OnCollisionEnter(Collision collision)
    {
        Explode();
        
        // Call base at the end because it deletes the gameobject
        base.OnCollisionEnter(collision);
    }
    
    protected virtual void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemies);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                var distance = (colliders[i].transform.position - transform.position).magnitude;
                var enemy = colliders[i].gameObject.GetComponent<Enemy>();
                var damage = (explosionRadius - distance) * maxExplosionDamage / explosionRadius;
                enemy.ChangeHealth(-damage);
            }
        }
        
        // Create explosion particles where the arrow exploded
        Instantiate(explodeParticles, transform.position, transform.rotation);
    }
}
