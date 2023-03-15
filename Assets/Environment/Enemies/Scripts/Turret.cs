using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected float lookSpeed;
    
    public void Attack()
    {
        weapon.Use();
    }

    public virtual void LookTowards(Vector3 lookDirection)
    {
        var weaponRotation = Quaternion.RotateTowards(
            weapon.transform.rotation,
            Quaternion.LookRotation(lookDirection),
            lookSpeed);
        
        Quaternion turretRotation = Quaternion.Euler(0, weaponRotation.eulerAngles.y, 0);
        transform.rotation = turretRotation;
        weapon.transform.rotation = weaponRotation;
    }
}
