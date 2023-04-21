using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Transform playerLookAt;
    
    // A transform that stores the main camera
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float health;
    [SerializeField] private float shield;
    [SerializeField] private LayerMask interactable;

    [Header("Audio")] 
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioSource rechargeSound;
    [SerializeField] private AudioSource collectAudio;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource damageSound;

    public UnityEvent onItemPickup;
    public UnityEvent onCrateUse;

    private bool _visible = false;
    private bool _showingVisible = false;
    private int _intuition;
    private Rigidbody playerRigidbody;
    private bool _paused = false;
    
    
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item Pickup"))
        {
            GameObject item = collision.gameObject;
            string itemName = item.GetComponent<ItemPickup>().itemName;
            if (Inventory.Instance.UpdateItemCount(itemName, 1))
            {
                Destroy(item);
                collectAudio.Play();
                onItemPickup.Invoke();
            }
        }    
    }
    
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

    public void UseObject()
    {
        FireUseRaycast();
    }

    private void FireUseRaycast()
    {
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);

        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 25, interactable))
        {
            Debug.Log(hit.collider.tag);

            if (hit.collider.CompareTag("Intuition Source"))
            {
                IntuitionSource intuitionSource = hit.collider.GetComponent<IntuitionSource>();

                if (intuitionSource != null)
                {
                    int rewardValue = intuitionSource.UseReward();

                    ChangeIntuition(rewardValue);
                    return;
                }
            }

            if (hit.collider.CompareTag("Final Crystal"))
            {
                FinalCrystal finalCrystal = hit.collider.GetComponent<FinalCrystal>();

                if (_intuition >= finalCrystal.GetRequiredIntuition() && !finalCrystal.Charged())
                {
                    finalCrystal.Charge();
                    ChangeIntuition(-finalCrystal.GetRequiredIntuition());
                }
            }

            if (hit.collider.CompareTag("Loot Crate"))
            {
                LootCrate lootCrate = hit.collider.GetComponent<LootCrate>();
                
                if (!lootCrate.Lootable()) 
                    return;

                string[] lootedItems = lootCrate.LootItems();
                foreach (string lootedItem in lootedItems)
                {
                    Inventory.Instance.UpdateItemCount(lootedItem, 1);
                }
                onCrateUse.Invoke();
            }

            if (hit.collider.CompareTag("Culdron"))
            {
                Culdron culdron = hit.collider.GetComponent<Culdron>();
                
                culdron.Use();
            }
        }
    }

    public bool ChangeIntuition(int amount)
    {
        if (amount > 0)
        {
            // Adding Intuition
            if (_intuition >= 0)
                _intuition += amount;
            else
                _intuition = 0;
            
            HUDController.Instance.SetIntuition(_intuition);
            return true;
        } 
        
        if (amount < 0)
        {
            // Removing Intuition
            if (_intuition + amount >= 0)
            {
                _intuition += amount;
                
                HUDController.Instance.SetIntuition(_intuition);
                return true;
            }
            
            HUDController.Instance.SetIntuition(_intuition);
            return false;
        }

        return false;
    }

    public void SetVisible()
    {
        _visible = true;
    }

    private void LateUpdate()
    {
        if (_visible && !_showingVisible)
        {
            HUDController.Instance.SetVisibleIndicator(true);
            _showingVisible = true;
        }
        else if (!_visible && _showingVisible)
        {
            HUDController.Instance.SetVisibleIndicator(false);
            _showingVisible = false;
        }

        _visible = false;
    }

    public void DealDamage(float amount)
    {
        if (amount <= 0)
            return;

        if (shield <= 0)
        {
            float newHealth = health - amount;
            if (newHealth > 0)
            {
                damageSound.Play();
                health = newHealth;
                HUDController.Instance.SetHealth(health);
                PostFXController.Instance.SetVignette(100 - health);
                PostFXController.Instance.SetChromaticAberration(100 - health);
            }
            else
            {
                health = 0;
                GameOver();
            }
        }
        else
        {
            float newShield = shield - amount;
            if (newShield > 0)
            {
                damageSound.Play();
                shield = newShield;
                HUDController.Instance.SetShield(shield);
            }
            else
            {
                shield = 0;
                HUDController.Instance.SetShield(shield);
                DealDamage(-newShield);
            }
        }
    }

    public void AddHealth(float amount)
    {
        if (amount <= 0 || health >= 100)
            return;
        
        var newHealth = health + amount;
        if (newHealth > 0 && newHealth <= 100)
        {
            health = newHealth;
            healSound.Play();
            HUDController.Instance.SetHealth(health);
            PostFXController.Instance.SetVignette(100 - health);
            PostFXController.Instance.SetChromaticAberration(100 - health);
        }
        else
        {
            health = 100;
            healSound.Play();
            HUDController.Instance.SetHealth(health);
            PostFXController.Instance.SetVignette(100 - health);
            PostFXController.Instance.SetChromaticAberration(100 - health);
        }
    }

    public void AddShield(float ammount)
    {
        if (ammount <= 0 || shield >= 100)
            return;
        
        var newShield = shield + ammount;
        if (newShield > 0 && newShield <= 100)
        {
            shield = newShield;
            rechargeSound.Play();
            HUDController.Instance.SetShield(shield);
        }
        else
        {
            shield = 100;
            rechargeSound.Play();
            HUDController.Instance.SetShield(shield);
        }
    }

    private void GameOver()
    {
        HUDController.Instance.GameOver();
        GetComponent<InputManager>().EnableRestart();
        audioListener.enabled = false;
        Time.timeScale = 0;
    }

    public void Restart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }
    }
    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_paused)
            {
                Time.timeScale = 1;
                _paused = false;
                audioListener.enabled = true;
                HUDController.Instance.SetPause(false);
                GetComponent<InputManager>().DisableRestart();
            }
            else
            {
                Time.timeScale = 0;
                _paused = true;
                audioListener.enabled = false;
                HUDController.Instance.SetPause(true);
                GetComponent<InputManager>().EnableRestart();
            }
        }
    }
    public void OnGameStart()
    {
        Debug.Log("start");
        playerRigidbody.useGravity = true;
        GetComponent<PlayerUpdatedMovement>().enabled = true;
        GetComponent<PlayerUpdatedController>().enabled = true;
        audioListener.enabled = true;

        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 600)) // May want to add a layermask to this
        {
            transform.position = hit.point + (Vector3.up * 0.5f);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield Source"))
        {
            MushroomCircle mushroomCircle = other.gameObject.GetComponent<MushroomCircle>();
            
            AddShield(mushroomCircle.Use());
        }
    }


}
