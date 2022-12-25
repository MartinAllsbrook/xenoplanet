using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    [SerializeField] private Bow _bow;
    [SerializeField] private Grapple _grapple;
    
    private Vector2 _moveDirection;
    private Vector2 _cameraDirection;
    private bool _isSprinting;
    private bool _isJumping;
    private bool _isCrouching;

    private bool readyToJump = false;
    private float _fireStrength;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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

    // private void OnMove(InputAction.CallbackContext value)
    // {
    //
    // }

    private void Update()
    {
        //Press A (Space) – Jump
        // _playerControls.Player.Jump.performed += context => _playerMovement.Jump();
        
        // When player is presses jump
        bool jump = _playerControls.Player.Jump.WasPressedThisFrame();

        //Press LeftStick (Shift) - Sprint
        float sprint = _playerControls.Player.Sprinting.ReadValue<float>();

        //Press RightStick (Control) - Crouch
        float crouch = _playerControls.Player.Crouch.ReadValue<float>();

        //Move LeftStick (WASD) – Move
        _moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();

        //_____Call Movement_____
        _playerMovement.PlayerInput(_moveDirection, jump);

        //Move RightStick (Mouse) – Camera
        if (_playerControls.Player.Camera.IsInProgress())
        {
            _cameraDirection = _playerControls.Player.Camera.ReadValue<Vector2>();
            _playerMovement.CameraControl(_cameraDirection);
        };

        // RT / LMB => Hold to charge arrow
        if (_playerControls.Player.Fire.IsInProgress())
        {
            _bow.ChargeArrow();
        }

        // RT / LMB => Release to fire arrow
        if (_playerControls.Player.Fire.WasReleasedThisFrame())
        {
            _bow.FireArrow();
        }
        
        // Y / Q => Cycle arrows
        if (_playerControls.Player.CycleArrows.WasPressedThisFrame())
        {
            _bow.CycleArrow();
        }
        
        // D-Pad Up => Shrink Grapple
        if (_playerControls.Player.PadUp.IsInProgress())
            _grapple.ChangeGrappleLength(true);

        // D-Pad Down => Lengthen Grapple
        if (_playerControls.Player.PadDown.IsInProgress())
            _grapple.ChangeGrappleLength(false);

        // LT / RMB => Release to delete grapple
        if (_playerControls.Player.Use.WasPressedThisFrame())
        {
            _grapple.Unhook();
        }

        // I dont know what this does
        // _playerControls.Player.Movement.canceled += context => _cameraDirection = Vector2.zero;
    }
}
