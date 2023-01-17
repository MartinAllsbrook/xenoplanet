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
    [SerializeField] protected float slowdownDistance;
    
    protected Vector3 currentVelocity;
    protected float locationTolerance;
    
    protected void Update()
    {
        locationTolerance = range[0];

        if (CanSeePlayer(out RaycastHit hit))
        {
            // Debug.Log(hit);
            targetLocation = hit.transform.position;
            MoveTo(targetLocation);
            if ((hit.transform.position - transform.position).magnitude < range[1]) 
                Attack();
            return;
        } 
        Idle();
        // Make raycast towards player
        /*Vector3 direction = Player.Instance.transform.position + new Vector3(0, 1, 0) - transform.position;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance, visible))
        {
            // If enemy can see player
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                targetLocation = hit.transform.position;
                MoveTo(targetLocation);
                if ((hit.transform.position - transform.position).magnitude < range[1]) 
                    Attack();
            }
            // Idle if unable to see player
            else
                Idle();
        }*/
    }

    protected virtual void Attack()
    { 
        transform.LookAt(Player.Instance.transform.position + Vector3.up);
    }

    protected void Idle()
    {
        var delta = targetLocation - transform.position;
        var direction = delta.normalized;
        var distance = delta.magnitude;
        if (distance > locationTolerance)
        {
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, speed / 2 * direction,
                ref currentVelocity, accelerationTime / 2);
            locationTolerance += Time.deltaTime * (locationTolerance + 0.1f);
        }

        /*else if (enemyRigidbody.velocity.magnitude > 0.1f)
        {
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, Vector3.zero, 
                ref currentVelocity, accelerationTime / 2);
        }*/
        else
        {
            targetLocation = GenerateRandomTarget();
            locationTolerance = 1;
        }
            
        // Debug.Log("Idle");
    }

    protected override Vector3 GenerateRandomTarget()
    {
        Vector3 position = new Vector3(
            transform.position.x + Random.Range(-idleDistance, idleDistance), 
            0, 
            transform.position.z + Random.Range(-idleDistance, idleDistance));

        // Set y comp of postition to account for the height of terrain;
        position.y = GetHeight(position) + 2 + Random.Range(hoverHeight[0], hoverHeight[1]);
        return position;
    }

    protected void MoveTo(Vector3 target)
    {
        // Debug.Log("Distance: " + (transform.position - targetLocation).magnitude + "Target: " + target);
        target.y += hoverHeight[0];
        var delta = target - transform.position;
        var direction = delta.normalized;
        var distance = delta.magnitude;
        if (distance > range[0])
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, speed * direction, ref currentVelocity, accelerationTime);
        else
            enemyRigidbody.velocity = Vector3.SmoothDamp(enemyRigidbody.velocity, Vector3.zero, ref currentVelocity, accelerationTime);
    }

    protected float GetHeight(Vector3 origin)
    {
        origin.y = 512;
        Ray ray = new Ray(origin, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, 513, visible);
        return hit.point.y; 
    }
}
