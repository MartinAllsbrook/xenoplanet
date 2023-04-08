using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Enemy : BreakableObject
{
    [SerializeField] protected float idleDistance;
    [SerializeField] protected float viewDistance;
    [SerializeField] protected float fov;

    [SerializeField] protected LayerMask visible;
    [SerializeField] private LayerMask enemies;
    [SerializeField] protected IndicatorLight canSeePlayerIndicator;
    
    [SerializeField] protected Turret turret;

    protected Vector3 targetLocation;
    protected Vector3 lastPlayerLocation;
    protected bool canSeePlayer;
    
    // private UnityEvent playerVisible;
    protected virtual void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // Start by generating a random target location to go to
        targetLocation = GenerateRandomTarget();
    }

    protected virtual void Update()
    {
        // Make indicator light solid if the player is visible
        canSeePlayerIndicator.Flashing = !canSeePlayer;
    }

    public override void ChangeHealth(float change)
    {
        base.ChangeHealth(change);
        if (health > 0)
            AlertEnemies();
    }

    private void AlertEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance, enemies);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                enemy.SetTarget(transform.position);
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        if (!canSeePlayer)
        {
            targetLocation = target;
        }
    }

    // Generate a random position
    protected virtual Vector3 GenerateRandomTarget()
    {
        Vector3 position = transform.position;
        return new Vector3(
            position.x + Random.Range(-idleDistance, idleDistance), 
            position.y + Random.Range(0, idleDistance),
            position.z + Random.Range(-idleDistance, idleDistance));
    }

    protected bool CanSeePlayer(out RaycastHit hitOut)
    {
        Vector3 direction = Player.Instance.playerLookAt.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance, visible))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 playerDirection = (hit.point - transform.position).normalized;
                var angle = Vector3.Angle(turret.transform.forward, playerDirection);
                if (angle < fov)
                {
                    // Debug.Log(angle);
                    canSeePlayer = true;
                    hitOut = hit;
                    return true;
                }
            }
        }
        canSeePlayer = false;
        hitOut = hit;
        return false;
    }
}
