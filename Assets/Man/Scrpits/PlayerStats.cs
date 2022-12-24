using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float health;

    public void DealDamage(float damage)
    {
        // Debug.Log("Damage: " + damage);
        health -= damage;
        HUDController.Instance.SetHealth(health);
    }
}
