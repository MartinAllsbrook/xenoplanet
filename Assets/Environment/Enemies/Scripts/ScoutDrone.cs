using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutDrone : FlyingEnemy
{
    protected override void Update()
    {
        locationTolerance = range[0];
        
        if (CanSeePlayer(out RaycastHit hit)) // If the enemy can currently see the player
        {
            Vector3 lookVector = Player.Instance.transform.position + Vector3.up - transform.position; // Add up so we're not looking at feet
            turret.LookTowards(lookVector);
            
            targetLocation = hit.transform.position; // Store player position
            if ((hit.transform.position - transform.position).magnitude < range[1]) // If player is within range
                turret.Attack();
            
            MoveTo(targetLocation); // Move to most recently stored target location, does this need to be in this area?
        }
        
        else // Else the enemy cannot see the player
        {
            Ray ray = new Ray(targetLocation, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit groundHit, 512, visible))
            {
                turret.LookTowards(groundHit.point + Vector3.up - transform.position);
            }
            Idle();
        }
    }
}
