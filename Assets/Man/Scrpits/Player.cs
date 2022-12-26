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
        if (health > 0)
            health -= damage;
        else
            health = 0;
        
        // Communicate new health
        HUDController.Instance.SetHealth(health);
        PostFXController.Instance.SetVignette(100 - health);
        PostFXController.Instance.SetChromaticAberration(100 - health);
    }
}
