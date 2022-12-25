using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleArrow : Arrow
{
    protected override void OnCollisionEnter(Collision collision)
    {
        // Set grapple as hooked
        Grapple.Instance.Hook(transform.position);
        base.OnCollisionEnter(collision);
    }
}
