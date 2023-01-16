using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutDrone : FlyingEnemy
{
    [SerializeField] private float coolDownTime;
    private float coolDown;

    [SerializeField] private GameObject laser;

    private void Start()
    {
        coolDown = coolDownTime;
    }

    protected override void Attack()
    {
        if (coolDown > 0)
            coolDown -= Time.deltaTime;
        else
        {
            coolDown = coolDownTime;
            FireLaser();
        }
    }

    private void FireLaser()
    {
        Instantiate(laser, transform.position + transform.forward, Quaternion.Euler(transform.rotation.eulerAngles));
    }
}
