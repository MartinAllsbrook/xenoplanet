using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveArrow : Arrow
{
    [SerializeField] private GameObject explodeParticles;

    protected override void OnCollisionEnter(Collision collision)
    {
        Instantiate(explodeParticles, transform.position, transform.rotation);
        base.OnCollisionEnter(collision);
    }
}
