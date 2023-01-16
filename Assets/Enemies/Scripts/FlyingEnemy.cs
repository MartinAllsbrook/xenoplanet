using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingEnemy : Enemy
{
    [SerializeField] protected float[] range = new float[2]; // range[0] = min range, range[1] = max range
    [SerializeField] protected float[] hoverHeight = new float[2]; // hoverHeight[0] = min hoverHeight, hoverHeight[1] = max hoverHeight
    
    [SerializeField] protected float speed;
    [SerializeField] protected float accelerationTime;
    
    protected Vector3 currentVelocity;
    protected float locationTolerance = 0;
    
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
                targetLocation = hit.transform.position;
                // Move to player and attack when in range
                MoveTo(targetLocation);
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
        transform.LookAt(Player.Instance.transform.position + Vector3.up);
    }

    protected void Idle()
    {
        if ((transform.position - targetLocation).magnitude > locationTolerance)
        {
            MoveTo(targetLocation);
            locationTolerance += Time.deltaTime * (locationTolerance + 0.1f);
        }
        else
        {
            targetLocation = GenerateRandomTarget();
            locationTolerance = 0;
        }
            
        Debug.Log("Idle");
    }

    protected override Vector3 GenerateRandomTarget()
    {
        return new Vector3(
            transform.position.x + Random.Range(-10f, 10f), 
            Random.Range(hoverHeight[0], hoverHeight[1]),
            transform.position.z + Random.Range(-10f, 10f));
    }

    protected void MoveTo(Vector3 target)
    {
        Debug.Log("Distance: " + (transform.position - targetLocation).magnitude + "Target: " + target);
        target.y += hoverHeight[0];
        var delta = target - transform.position;
        var direction = delta.normalized;
        var distance = delta.magnitude;
        enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, speed * direction, ref currentVelocity, accelerationTime);

        // if (distance > range[0])
        //     enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, speed * direction, ref currentVelocity, accelerationTime);
        // else
        //     enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, Vector3.zero, ref currentVelocity, accelerationTime);
    }
}
