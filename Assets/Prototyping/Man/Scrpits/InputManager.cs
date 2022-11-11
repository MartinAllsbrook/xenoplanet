using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    
    private Vector2 _moveDirection;
    private Vector2 _cameraDirection;
    private bool _isSprinting;
    private bool _isJumping;
    private bool _isCrouching;
    


    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    }

    private void FixedUpdate()
    {
        //Press A (Space) – Jump
        _playerControls.Player.Jump.performed += context => _playerMovement.Jump();

        //Press LeftStick (Shift) - Sprint
        float sprint = _playerControls.Player.Sprinting.ReadValue<float>();
        
        //Press RightStick (Control) - Crouch
        float crouch = _playerControls.Player.Crouch.ReadValue<float>();

        //Move LeftStick (WASD) – Move
        _moveDirection = _playerControls.Player.Movement.ReadValue<Vector2>();
        _playerMovement.Move(_moveDirection);
        Debug.Log(_moveDirection);
        
        //Move RightStick (Mouse) – Camera
        _playerControls.Player.Camera.performed += context =>
        { 
            _cameraDirection = context.ReadValue<Vector2>();
            _playerMovement.CameraControl(_cameraDirection);
        };
        _playerControls.Player.Movement.canceled += context => _cameraDirection = Vector2.zero;
    }
}
