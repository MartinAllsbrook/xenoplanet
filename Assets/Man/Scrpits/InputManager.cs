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
    
    public Vector2 moveDirection;
    private Vector2 _cameraDirection;
    private bool _isSprinting;
    private bool _isJumping;
    private bool _isCrouching;

    private bool readyToJump = false;
    private float _fireStrength;

    private bool inventoryOpen = false;

    public UnityEvent toggleInventory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (toggleInventory == null)
            toggleInventory = new UnityEvent();
        
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
        if (inventoryOpen)
        {
            InventoryControlChecks();
        }
        else
        {
            BasicControlChecks();
        }
    }

    private void BasicControlChecks()
    {
                //Press A (Space) – Jump
        // _playerControls.Player.Jump.performed += context => _playerMovement.Jump();
        
        // When player is presses jump
        bool jump = _playerControls.Player.Jump.WasPressedThisFrame();

        //Press LeftStick (Shift) - Sprint
        bool sprint = _playerControls.Player.Sprinting.IsInProgress();

        //Press RightStick (Control) - Crouch
        bool crouch = _playerControls.Player.Crouch.IsInProgress();

        //Move LeftStick (WASD) – Move
        moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();

        //_____Call Movement_____
        _playerMovement.PlayerInput(moveDirection, jump, sprint, crouch);

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

        // D-Pad Up => Shrink Grapple
        if (_playerControls.Player.PadUp.IsInProgress())
            _grapple.ChangeGrappleLength(true);

        // D-Pad Down => Lengthen Grapple
        if (_playerControls.Player.PadDown.IsInProgress())
            _grapple.ChangeGrappleLength(false);

        // LT / RMB => Release to delete grapple
        if (_playerControls.Player.Use.WasPressedThisFrame())
            _grapple.Unhook();
        
        // D-Pad Left => toggle inventory
        if (_playerControls.Player.PadLeft.WasPerformedThisFrame())
            ToggleInventory();
    }

    private void InventoryControlChecks()
    {
        moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();

        // D-Pad Left => toggle inventory
        if (_playerControls.Player.PadLeft.WasPerformedThisFrame())
            ToggleInventory();
    }

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
