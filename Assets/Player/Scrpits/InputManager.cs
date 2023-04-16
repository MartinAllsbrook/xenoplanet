using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    // public static InputManager Instance;
    
    // private PlayerControls _playerControls;
    private PlayerInput _playerInput;
    
    // private bool _readyToJump = false;
    // private bool _inventoryOpen = false;
    // private float _fireStrength;

    private void Awake()
    {
        // Create singleton..... TODO: REMOVE THIS LMAO 
        /*if (Instance == null)
            Instance = this;*/

        _playerInput = GetComponent<PlayerInput>(); 
        // _playerControls = new PlayerControls();
        // _fireStrength = 0;
    }

    /*private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    }*/

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
        // ControlChecks();
    }

    /*private void ControlChecks()
    {
        // if (_playerControls.Player.PadRight.WasPerformedThisFrame())
        //     hotbarNext.Invoke();
        //
        // if (_playerControls.Player.PadLeft.WasPerformedThisFrame())
        //     hotbarPrev.Invoke();
        //
        // // LT / RMB => Release to delete grapple
        // if (_playerControls.Player.Use.WasPressedThisFrame())
        //     useItem.Invoke();
        //     // _grapple.Unhook();
        //
        // // D-Pad Down => toggle inventory
        // if (_playerControls.Player.PadDown.WasPerformedThisFrame())
        //     ToggleInventory();

        if (_playerControls.Player.OpenInventory.WasPerformedThisFrame())
        {
            Debug.Log("Open Inventory");
            _playerInput.SwitchCurrentActionMap("Inventory");
            // _playerInput.actions["Player"].Disable();
            // _playerControls.Player.Disable();
            // _playerControls.Inventory.Enable();
        }

        if (_playerControls.Inventory.CloseInventory.WasPerformedThisFrame())
        {
            Debug.Log("Close Inventory");
            _playerInput.SwitchCurrentActionMap("Player");
            // _playerControls.Player.Enable();
            // _playerControls.Inventory.Disable();
        }
            // _playerControls.actions("Player");
    }*/

    public void CloseInventory(InputAction.CallbackContext context)
    {
        if (context.started)
            _playerInput.SwitchCurrentActionMap("Player");
        
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
            _playerInput.SwitchCurrentActionMap("Inventory");
    }
    
    public void EnableRestart()
    {
        _playerInput.SwitchCurrentActionMap("GameOver");
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
    
    
    /*private void ToggleInventory()
    {
        // Call event
        toggleInventory.Invoke();
        
        // Set inventory open
        if (inventoryOpen)
            inventoryOpen = false;
        else
            inventoryOpen = true;
    }*/
}
