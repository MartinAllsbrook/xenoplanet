using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveArrow : Arrow
{
    [SerializeField] private GameObject explodeParticles;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionFalloff;
    [SerializeField] private float maxExplosionDamage;
    protected override void OnCollisionEnter(Collision collision)
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemies);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                var distance = (colliders[i].transform.position - transform.position).magnitude;
                var enemy = colliders[i].gameObject.GetComponent<Enemy>();
                var damage = (explosionRadius - distance) * maxExplosionDamage / explosionRadius;
                enemy.Health = -damage;
            }
        }
        
        // Create explosion particles where the arrow exploded
        Instantiate(explodeParticles, transform.position, transform.rotation);
        
        // Call base at the end because it deletes the gameobject
        base.OnCollisionEnter(collision);
    }
}
