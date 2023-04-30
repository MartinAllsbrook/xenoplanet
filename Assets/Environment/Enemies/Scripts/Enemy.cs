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
    [SerializeField] protected GameObject beamer;
    [SerializeField] protected float beamerSpawnRadius;
    [SerializeField] protected float maxBeamerSpawnTime;
    
    protected float _beamerTimer;
    
    protected Vector3 targetLocation;
    protected Vector3 lastPlayerLocation;
    protected bool canSeePlayer;
    
    // private UnityEvent playerVisible;
    protected virtual void Awake()
    {
        base.Awake();
        _beamerTimer = maxBeamerSpawnTime;
    }

    private void Start()
    {
        // Start by generating a random target location to go to
        targetLocation = GenerateRandomTarget();
        
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 150))
        {
            transform.position = hit.point + Vector3.up * 4;
        }
    }

    protected virtual void Update()
    {
        // Make indicator light solid if the player is visible
        // canSeePlayerIndicator.Flashing = !canSeePlayer;
        if (_beamerTimer > 0)
        {
            _beamerTimer -= Time.deltaTime;
        }
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
    
    private void AlertEnemies(Vector3 position)
    {
        if (_beamerTimer <= 0)
        {
            Instantiate(beamer,
                transform.position + new Vector3(Random.Range(-beamerSpawnRadius, beamerSpawnRadius), 125,
                    Random.Range(-beamerSpawnRadius, beamerSpawnRadius)), new Quaternion(0, 0, 0, 0));

            _beamerTimer = maxBeamerSpawnTime;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance, enemies);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                enemy.SetTarget(position);
                enemy.SetCanSeePlayer(true);
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        if (!canSeePlayer)
        {
            Debug.Log("setTarget");
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

    public void SetCanSeePlayer(bool canSee)
    {
        canSeePlayer = canSee;
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
                    AlertEnemies(hit.point);
                    Player.Instance.SetVisible();
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
