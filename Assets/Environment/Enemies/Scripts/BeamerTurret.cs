using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamerTurret : Turret
{
    public override void LookTowards(Vector3 lookDirection)
    {
        var weaponRotation = Quaternion.RotateTowards(
            weapon.transform.rotation,
            Quaternion.LookRotation(lookDirection),
            lookSpeed);
        
        transform.rotation = weaponRotation;
    }
}
