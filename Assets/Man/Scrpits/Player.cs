using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float health;

    private Rigidbody playerRigidbody;
    // [SerializeField] private int itemPickup;
    
    private void Awake()
    {
        // Create player singleton
        if (Instance == null)
            Instance = this;

        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        TerrainLoader.Instance.terrainReady.AddListener(OnGameStart);
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
        if (collision.gameObject.CompareTag("Item Pickup"))
        {
            Debug.Log("Picked up item");
            ItemPickup itemPickup = collision.gameObject.GetComponent<ItemPickup>();
            HUDController.Instance.PickUpItem(itemPickup.Item);
        }
    }

    private void OnGameStart()
    {
        playerRigidbody.useGravity = true;


        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 600)) // May want to add a layermask to this
        {
            transform.position = hit.point + (Vector3.up * 0.5f);
        }
    }
}
