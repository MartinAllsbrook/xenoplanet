using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlackArrow : ExplosiveArrow
{
    [SerializeField] private float flackTriggerTime;
    private bool triggered = false;
    private void FixedUpdate()
    {
        // Find enemies near the arrow
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemies);
        // If there are any enemies near the arrow tigger explosive
        if (colliders.Length > 0 && triggered == false)
        {
            StartCoroutine(FlackExplode());
            triggered = true; // Set triggered to true so the arrow does not explode again
        }
    }

    private IEnumerator FlackExplode()
    {
        // Wait a little bit to get closer to enemy as this coroutine will be triggered as soon as the enemy enters the explosion radius
        yield return new WaitForSeconds(flackTriggerTime);
        Explode();
        
        // Must also destroy the gameObject because Explode() does not destroy it
        Destroy(gameObject);
        
        yield return null;
    }
}
