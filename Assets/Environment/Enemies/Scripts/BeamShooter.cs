using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeamShooter : Weapon
{
    [SerializeField] private float chargeTimeCoefficient;
    [SerializeField] private float chargeExponent;
    [SerializeField] private float widthCoefficient;
    [SerializeField] private float damageCoefficient;
    
    private LineRenderer _laserBeam;
    private float _chargeTime;
    private bool _firingLaser;

    private void Awake()
    {
        _laserBeam = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!_firingLaser && _chargeTime != 0)
        {
            _chargeTime = 0;
            _laserBeam.widthMultiplier = 0;
            _laserBeam.SetPosition(0, transform.position);
            _laserBeam.SetPosition(1, transform.position);
        }
        _firingLaser = false;
    }

    public override void Use()
    {
        _firingLaser = true;
        _chargeTime += Time.deltaTime;
        
        // Set line renderer's positions
        _laserBeam.SetPosition(0, transform.position);
        _laserBeam.SetPosition(1, Player.Instance.transform.position + Vector3.up);
        
        float laserCharge = 1 - 1 / (Mathf.Pow(_chargeTime * chargeTimeCoefficient, chargeExponent) + 1);
        // Set line renderer's width
        _laserBeam.widthMultiplier = widthCoefficient * laserCharge * Random.Range(0.85f, 1.0f);
        Player.Instance.DealDamage(damageCoefficient * laserCharge);
    }
}
