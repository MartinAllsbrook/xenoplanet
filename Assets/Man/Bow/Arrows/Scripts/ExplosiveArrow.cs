using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveArrow : Arrow
{
    [SerializeField] private GameObject explodeParticles;

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(explodeParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
