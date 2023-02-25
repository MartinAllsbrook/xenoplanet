using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    // A transform that stores the main camera
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float health;

    private int _intuition;
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
        // TerrainLoader.Instance.terrainReady.AddListener(OnGameStart); // Add game start event listener
    }

    public void UseObject(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            FireRaycast();
        }
    }

    private void FireRaycast()
    {
        // Create a ray with the origin and direction of the main camera
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);

        // Create a variable to store the hit information
        RaycastHit hit;

        // Check if the ray hits anything
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has the tag "Rock Dude"
            if (hit.collider.CompareTag("Rock Dude"))
            {
                // Get the ManaReward component of the hit object
                RockDude rockDude = hit.collider.GetComponent<RockDude>();

                // Check if the ManaReward component is not null
                if (rockDude != null)
                {
                    // Call the UseReward method and store the value
                    int rewardValue = rockDude.UseReward();

                    // Call another function called SetMana and pass it the reward value
                    ChangeIntuition(rewardValue);
                }
            }
        }
    }

    private void ChangeIntuition(int ammount)
    {
        if (_intuition >= 0)
        {
            _intuition += ammount;
        }
        else
            _intuition = 0;
        
        HUDController.Instance.SetIntuition(_intuition);
    }

    
    public void ChangeHealth(float ammount)
    {
        var newHealth = health + ammount;
        Debug.Log(ammount);
        if (newHealth > 0 && newHealth <= 100)
            health = newHealth;
        else if (newHealth > 100)
        {
            health = 100;
        }
        else if (newHealth <= 0)
        {
            health = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
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

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item Pickup"))
        {
            // ItemPickup itemPickup = other.gameObject.GetComponent<ItemPickup>();
            string itemName = other.GetComponent<ItemPickup>().GetName();
            Debug.Log("Picked up: " + itemName);
            HUDController.Instance.PickUpItem(itemName);
        }
    }*/

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item Pickup"))
        {
            Debug.Log("Picked up item");
            ItemPickup itemPickup = collision.gameObject.GetComponent<ItemPickup>();
            HUDController.Instance.PickUpItem(itemPickup.Item);
        }
    }*/

    private void OnGameStart()
    {
        Debug.Log("start");
        playerRigidbody.useGravity = true;
        
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 600)) // May want to add a layermask to this
        {
            transform.position = hit.point + (Vector3.up * 0.5f);
        }
    }

    /*private void OnSpotted()
    {
        Debug.Log("Player Spotted");
    }

    private void OnHidden()
    {
        Debug.Log("Player Hidden");
    }*/
}
