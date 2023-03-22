using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private float meleeOffset;
    [SerializeField] private float meleeRadius;
    [SerializeField] private float meleeDamage;
    
    public void Melee()
    {
        Debug.Log("melee");
        Vector3 center = transform.position + transform.forward * meleeOffset;
        
        Collider[] colliders = Physics.OverlapSphere(center, meleeRadius);
        if (colliders.Length > 0) 
        {
            foreach (var col in colliders)
            {
                if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Breakable Environment"))
                    col.transform.gameObject.GetComponent<BreakableObject>().Health = -meleeDamage;
            }
        }
    }
}
