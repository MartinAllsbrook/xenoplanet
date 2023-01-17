using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingEnemy : Enemy
{
    [SerializeField] protected float[] range = new float[2]; // range[0] = min range, range[1] = max range
    [SerializeField] protected float[] hoverHeight = new float[2]; // hoverHeight[0] = min hoverHeight, hoverHeight[1] = max hoverHeight
    
    [SerializeField] protected float speed;
    [SerializeField] protected float accelerationTime;
    [SerializeField] protected float slowdownDistance;
    [SerializeField] protected float lookSpeed;
    
    [SerializeField] protected LaserGun laserGun;

    protected Vector3 currentVelocity;
    protected float locationTolerance;
    
    protected void Update()
    {
        locationTolerance = range[0];
        // If the enemy can currently see the player
        if (CanSeePlayer(out RaycastHit hit))
        {
            // Debug.Log(hit);
            var rotation = Quaternion.RotateTowards(
                laserGun.transform.rotation,
                Quaternion.LookRotation(Player.Instance.transform.position + Vector3.up - transform.position),
                lookSpeed);
            laserGun.transform.rotation = rotation; 
            targetLocation = hit.transform.position;
            if ((hit.transform.position - transform.position).magnitude < range[1])
                Attack();
            MoveTo(targetLocation);

        }
        // Else the enemy cannot see the player
        else
        {
            Ray ray = new Ray(targetLocation, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit groundHit, 512, visible))
            {
                var rotation = Quaternion.RotateTowards(
                    laserGun.transform.rotation,
                    Quaternion.LookRotation(groundHit.point - transform.position),
                    lookSpeed);
                laserGun.transform.rotation = rotation; 
                // laserGun.transform.LookAt(groundHit.point);
            }
            Idle();
        }        
        
    }

    protected virtual void Attack()
    { 
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
        else
        {
            targetLocation = GenerateRandomTarget();
            locationTolerance = 1;
        }
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
