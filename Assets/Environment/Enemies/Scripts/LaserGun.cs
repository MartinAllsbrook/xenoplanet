using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LaserGun : MonoBehaviour
{
    [SerializeField] private GameObject laser;

    private float coolDown;
    [SerializeField] private float coolDownTime;

    protected void Start()
    {
        coolDown = coolDownTime;
    }
    
    public void Charge()
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
