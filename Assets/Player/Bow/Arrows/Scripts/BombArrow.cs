using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : ExplosiveArrow
{
    protected override void OnCollisionEnter(Collision collision)
    {
        Explode();
        
        // Call base at the end because it deletes the gameobject
        base.OnCollisionEnter(collision);
    }
}
