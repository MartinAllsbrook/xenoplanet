using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LaserGun : Weapon
{
    [SerializeField] private GameObject laser;
    private float coolDown;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float maxInaccuracy;
    // [SerializeField] private AudioSource fireAudio;
    
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
        Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles);
        rotation = OffsetQuaternion(rotation);
        // Vector3 position = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        Instantiate(laser, transform.position, rotation);
        // fireAudio.Play();
    }
    
    private Quaternion OffsetQuaternion(Quaternion inputQuaternion)
    {
        float horizontalOffset = Random.Range(-maxInaccuracy, maxInaccuracy);
        float verticalOffset = Random.Range(-maxInaccuracy, maxInaccuracy);
        Quaternion horizontalQuaternion = Quaternion.AngleAxis(horizontalOffset, Vector3.up);
        Quaternion verticalQuaternion = Quaternion.AngleAxis(verticalOffset, Vector3.right);
        return inputQuaternion * horizontalQuaternion * verticalQuaternion;
    }
}
