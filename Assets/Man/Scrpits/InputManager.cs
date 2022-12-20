using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    private PlayerActions _playerActions;
    
    private Vector2 _moveDirection;
    private Vector2 _cameraDirection;
    private bool _isSprinting;
    private bool _isJumping;
    private bool _isCrouching;
    
    float _fireStrength;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerActions = GetComponent<PlayerActions>();
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
        float jump = _playerControls.Player.Jump.ReadValue<float>();

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

        //Pull Bow
        if (_playerControls.Player.Fire.IsInProgress())
        {
            _playerActions.ChargeArrow();
        }
        
        //Shoot Bow
        if (_playerControls.Player.Fire.WasReleasedThisFrame())
        {
            _playerActions.FireArrow();
        }
    }
}
