using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] protected float[] range = new float[2]; // range[0] = min range, range[1] = max range
    [SerializeField] protected float[] hoverHeight = new float[2]; // hoverHeight[0] = min hoverHeight, hoverHeight[1] = max hoverHeight
    
    [SerializeField] protected float speed;
    [SerializeField] protected float accelerationTime;
    
    protected Vector3 currentVelocity;

    protected void Update()
    {
        // Make raycast towards player
        Vector3 direction = Player.Instance.transform.position + new Vector3(0, 1, 0) - transform.position;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance, visible))
        {
            // If enemy can see player
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                // Move to player and attack when in range
                MoveTo(hit.transform.position);
                if ((hit.transform.position - transform.position).magnitude < range[1]) 
                    Attack();
            }
            // Idle if unable to see player
            else
                Idle();
            
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("No Attack Implemented");
    }

    protected void Idle()
    {
        Debug.Log("Idle");
    }

    protected void MoveTo(Vector3 target)
    {
        target.y += hoverHeight[0];
        var delta = target - transform.position;
        var direction = delta.normalized;
        var distance = delta.magnitude;
        transform.LookAt(Player.Instance.transform.position + Vector3.up);
        if (distance > range[0])
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, speed * direction, ref currentVelocity, accelerationTime);
        else
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, Vector3.zero, ref currentVelocity, accelerationTime);
    }
}
