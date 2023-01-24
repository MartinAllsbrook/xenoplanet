using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float health;
    
    private Rigidbody playerRigidbody;
    /*public UnityEvent playerVisible;
    private bool playerSpotted;
    public bool PlayerSpotted
    {
        get { return playerSpotted; }
        set
        {
            // if enemies just lost sight of players
            if (playerSpotted && !value)
                OnHidden();
            // if enemies just gained sight of player
            else if (!playerSpotted && value)
                OnSpotted();
        }
    }*/
    
    private void Awake()
    {
        if (Instance == null) 
            Instance = this; // Create player singleton

        /*if (playerVisible == null)
            playerVisible = new UnityEvent();
        playerVisible.AddListener(OnPlayerVisible);*/
        
        playerRigidbody = GetComponent<Rigidbody>(); // Find rigid body
    }

    private void Start()
    {
        TerrainLoader.Instance.terrainReady.AddListener(OnGameStart); // Add game start event listener
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

    /*private void LateUpdate()
    {
        if (playerSpotted)
            PlayerSpotted = false;
        else
            playerSpotted = false;
    }

    private void OnPlayerVisible()
    {
        if (playerSpotted == false)
        {
            OnSpotted();
            playerSpotted = true;
        }
    }*/
    
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

    private void OnSpotted()
    {
        Debug.Log("Player Spotted");
    }

    private void OnHidden()
    {
        Debug.Log("Player Hidden");
    }
}
