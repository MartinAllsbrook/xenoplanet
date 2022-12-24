using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] private float chargeMultiplier;
    [SerializeField] private float roatationSpeed;
    private GameObject cannon;
    private LineRenderer _laserBeam;

    private float _lasercharge = 0;
    private bool _canSeePlayer = false;
    private Quaternion _targetRotation;
    private Vector3 _transformPosition;
    
    private void Awake()
    {
        cannon = transform.GetChild(0).gameObject;
        _laserBeam = cannon.GetComponent<LineRenderer>();
        _transformPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 direction = Bow.Instance.transform.position + new Vector3(0, 1, 0) - _transformPosition;
        Ray ray = new Ray(_transformPosition, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, range, visible))
        {
            Debug.Log(hit.transform.gameObject.tag);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                _targetRotation = Quaternion.LookRotation(direction);
                cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, _targetRotation, roatationSpeed);
                FireLaser(cannon.transform.forward);
                return;
            }
            else
                StopLaser();
        }

        if (cannon.transform.rotation != _targetRotation)
        {
            cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, _targetRotation, roatationSpeed);
        }
    }

    private void FireLaser(Vector3 direction)
    {
        // Cast ray for laser
        Ray laserRay = new Ray(_transformPosition, direction);
        Physics.Raycast(laserRay, out RaycastHit laserHit, range, visible);
        
        // Set line renderer's positions
        Vector3[] positions = new Vector3[2];
        positions[0] = _transformPosition; // Laser starts at the turret's location
        positions[1] = laserHit.point; // Laser ends at hit position of ray
        _laserBeam.SetPositions(positions);
        
        // Set line renderer's width
        _lasercharge += Time.deltaTime * chargeMultiplier;
        _laserBeam.widthMultiplier = _lasercharge;
        
        // If laser hit's the player deal damage
        GameObject objectHit = laserHit.transform.gameObject;
        if (objectHit.CompareTag("Player"))
        {
            PlayerStats playerStats = objectHit.GetComponent<PlayerStats>();
            playerStats.DealDamage(_lasercharge);
        }
    }

    private void StopLaser()
    {
        // Set laser charge
        _lasercharge = 0;
        _laserBeam.widthMultiplier = _lasercharge;
    }

    // private void OnDrawGizmos()
    // {
    //     Ray ray = new Ray(transform.position, Bow.Instance.transform.position - transform.position);
    //     Gizmos.DrawRay(ray);    
    // }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
