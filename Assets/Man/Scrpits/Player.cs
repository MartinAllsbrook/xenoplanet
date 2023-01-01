using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float health;
    [SerializeField] private int itemPickup;
    
    private void Awake()
    {
        // Create player singleton
        if (Instance == null)
            Instance = this;
    }

    public void ChangeHealth(float ammount)
    {
        if (health > 0)
            health += ammount;
        else
            health = 0;
        
        // Communicate new health
        HUDController.Instance.SetHealth(health);
        PostFXController.Instance.SetVignette(100 - health);
        PostFXController.Instance.SetChromaticAberration(100 - health);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == itemPickup)
        {
            Debug.Log("Picked up Item");
            if (collision.gameObject.CompareTag("Scrap Metal"))
            {
                ChangeHealth(10);
                Debug.Log("Picked up scrap");
            }
        }
    }
}
