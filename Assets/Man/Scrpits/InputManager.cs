using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    private PlayerControls _playerControls;
    [SerializeField] private Grapple _grapple;

    private bool readyToJump = false;
    private float _fireStrength;

    private bool inventoryOpen = false;

    public UnityEvent toggleInventory;
    public UnityEvent select;
    public UnityEvent hotbarNext;
    public UnityEvent hotbarPrev;
    public UnityEvent useItem;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        if (toggleInventory == null) toggleInventory = new UnityEvent();
        if (select == null) select = new UnityEvent();
        if (hotbarNext == null) hotbarNext = new UnityEvent();
        if (hotbarPrev == null) hotbarPrev = new UnityEvent();
        if (useItem == null) useItem = new UnityEvent();
        
        _playerControls = new PlayerControls();
        _fireStrength = 0;
    }

    private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    }

    private void Start()
    {
        // _moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();
        // _playerMovement.Move(_moveDirection);
        //
        // _playerControls.Player.Movement.performed += context =>
        // {
        //     _moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();
        //     _playerMovement.Move(_moveDirection);
        // };
        // if (_playerControls.Player.Movement.IsInProgress())
        // {
        //     _moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();
        //     _playerMovement.Move(_moveDirection);
        // }
    }
    
    private void Update()
    {
        // if (inventoryOpen)
        // {
        //     InventoryControlChecks();
        // }
        // else
        // {
        //     BasicControlChecks();
        // }
    }

    private void BasicControlChecks()
    {
        if (_playerControls.Player.PadRight.WasPerformedThisFrame())
            hotbarNext.Invoke();
        
        if (_playerControls.Player.PadLeft.WasPerformedThisFrame())
            hotbarPrev.Invoke();
        
        // LT / RMB => Release to delete grapple
        if (_playerControls.Player.Use.WasPressedThisFrame())
            useItem.Invoke();
            // _grapple.Unhook();
        
        // D-Pad Down => toggle inventory
        if (_playerControls.Player.PadDown.WasPerformedThisFrame())
            ToggleInventory();
    }

    // Control checks while inventory is open
    /*private void InventoryControlChecks()
    {
        moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();
        
        if (_playerControls.Player.Jump.WasPressedThisFrame())
            select.Invoke();

        // D-Pad Down => toggle inventory
        if (_playerControls.Player.PadDown.WasPerformedThisFrame())
            ToggleInventory();
    }*/

    // Toggles inventory state on and off
    private void ToggleInventory()
    {
        // Call event
        toggleInventory.Invoke();
        
        // Set inventory open
        if (inventoryOpen)
            inventoryOpen = false;
        else
            inventoryOpen = true;
    }
}
