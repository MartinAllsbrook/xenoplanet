using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LaserGun : Weapon
{
    [SerializeField] private GameObject laser;

    private float coolDown;
    [SerializeField] private float coolDownTime;

    protected void Start()
    {
        coolDown = coolDownTime;
    }
    
    public override void Use()
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
        Instantiate(laser, transform.position + transform.forward * 1.5f, Quaternion.Euler(transform.rotation.eulerAngles));
    }
    
}
