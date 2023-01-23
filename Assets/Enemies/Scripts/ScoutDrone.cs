using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutDrone : FlyingEnemy
{
    protected override void Attack()
    {
        base.Attack();
        // laserGun.transform.rotation = Quaternion.LookRotation();
        laserGun.Charge();
    }
}
