using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float health;

    private void Awake()
    {
        // Create player singleton
        if (Instance == null)
            Instance = this;
    }

    public void DealDamage(float damage)
    {
        // Debug.Log("Damage: " + damage);
        health -= damage;
        HUDController.Instance.SetHealth(health);
    }
}
