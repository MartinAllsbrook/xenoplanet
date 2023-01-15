using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTurret : Enemy
{
    [SerializeField] private float chargeMultiplier;
    [SerializeField] private float roatationSpeed;
    
    private GameObject cannon;
    private LineRenderer _laserBeam;

    private float _laserCharge = 0;
    private bool _canSeePlayer = false;
    private Quaternion _targetRotation;
    private Vector3 _transformPosition;
    
    private void Awake()
    {
        cannon = transform.GetChild(0).gameObject;
        _laserBeam = cannon.GetComponent<LineRenderer>();
        _transformPosition = cannon.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 direction = Player.Instance.transform.position + new Vector3(0, 1, 0) - _transformPosition;
        Ray ray = new Ray(_transformPosition, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance, visible))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                _targetRotation = Quaternion.LookRotation(direction);
                cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, _targetRotation, roatationSpeed);
                FireLaser(cannon.transform.forward);
                return;
            }
            // else (because of return)
            // StopLaser();
        }

        if (cannon.transform.rotation != _targetRotation)
        {
            cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, _targetRotation, roatationSpeed);
        }
    }

    private void FireLaser(Vector3 direction)
    {
        // Create vector3 to store the position the laser should end
        Vector3 laserEnd;

        // Cast ray for laser
        Ray laserRay = new Ray(_transformPosition, direction);
        if (Physics.Raycast(laserRay, out RaycastHit laserHit, viewDistance, visible))
        {
            // If laser hit's the player deal damage
            GameObject objectHit = laserHit.transform.gameObject;
            if (objectHit.CompareTag("Player"))
            {
                Player playerStats = objectHit.GetComponent<Player>();
                playerStats.ChangeHealth(-_laserCharge);
            }

            _laserCharge += (1 - _laserCharge) * Time.deltaTime * chargeMultiplier;

            laserEnd = laserHit.point; // Laser ends at hit position of ray
        }
        else
        {
            StopLaser();
            laserEnd = _transformPosition + (direction.normalized * viewDistance);
        }

        // Set line renderer's positions
        _laserBeam.SetPosition(0, _transformPosition);
        _laserBeam.SetPosition(1, laserEnd);
        
        // Set line renderer's width
        // _laserCharge += (1 - _laserCharge) * Time.deltaTime * chargeMultiplier;
        _laserBeam.widthMultiplier = _laserCharge * Random.Range(0.85f, 1.0f);
    }

    private void StopLaser()
    {
        // Set laser charge
        _laserCharge = 0;
        _laserBeam.widthMultiplier = _laserCharge;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Debug.Log("Turret Collided");
    }
}
