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
    private PlayerMovement _playerMovement;
    [SerializeField] private Bow _bow;
    [SerializeField] private Grapple _grapple;
    
    // public Vector2 moveDirection;
    private Vector2 _cameraDirection;
    // private bool _isSprinting;
    // private bool _isJumping;
    // private bool _isCrouching;

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
        _playerMovement = GetComponent<PlayerMovement>();
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

        //Move RightStick (Mouse) – Camera
        if (_playerControls.Player.Camera.IsInProgress())
        {
            _cameraDirection = _playerControls.Player.Camera.ReadValue<Vector2>();
            _playerMovement.CameraControl(_cameraDirection);
        };

        // RT / LMB => Hold to charge arrow
        if (_playerControls.Player.Fire.IsInProgress())
            _bow.ChargeArrow();

        // RT / LMB => Release to fire arrow
        if (_playerControls.Player.Fire.WasReleasedThisFrame())
            _bow.FireArrow();

        // Y / Q => Cycle arrows
        if (_playerControls.Player.CycleArrows.WasPressedThisFrame())
            _bow.CycleArrow();

        if (_playerControls.Player.Melee.WasPerformedThisFrame())
            _bow.Melee();

            /*// D-Pad Up => Shrink Grapple
            if (_playerControls.Player.PadUp.IsInProgress())
                _grapple.ChangeGrappleLength(true);
            
            // D-Pad Down => Lengthen Grapple
            if (_playerControls.Player.PadDown.IsInProgress())
                _grapple.ChangeGrappleLength(false);*/

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
        
        if (_playerControls.Player.FireGrapple.WasPerformedThisFrame())
            _grapple.FireGrapple();
        
        if (_playerControls.Player.FireGrapple.WasReleasedThisFrame())
            _grapple.Unhook();
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
