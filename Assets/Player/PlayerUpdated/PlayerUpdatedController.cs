using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerUpdatedController : MonoBehaviour
{
    #region Script References
    private PlayerUpdatedMovement _playerMovement;
    private PlayerUpdatedChecks _playerChecks;
    private PlayerUpdatedAnimations _playerAnimations;
    private PlayerUpdatedBow _playerBow;
    private PlayerCameraController _playerCameraController;
    private MeleeController _meleeController;
    private Player _player;
    #endregion

    #region Player Referances
    public CinemachineVirtualCamera moveCamera;
    public CinemachineVirtualCamera aimCamera;
    public Camera mainCamera;
    #endregion

    #region Input Values
    private bool _jumpInput;
    private Vector2 _moveInput;
    private bool _sprintInput;
    private bool _crouchInput;
    private Vector2 _cameraInput;
    private bool _aimingInput;
    #endregion

    //Events
    [SerializeField] UnityEvent OnJumpEvent;
    [SerializeField] UnityEvent OnLandEvent;
    [SerializeField] UnityEvent OnSprintEvent;
    [SerializeField] UnityEvent OnCrouchEvent;
    
    void Start()
    {
        _playerMovement = GetComponent<PlayerUpdatedMovement>();
        _playerChecks = GetComponent<PlayerUpdatedChecks>();
        _playerAnimations = GetComponent<PlayerUpdatedAnimations>();
        _playerBow = GetComponent<PlayerUpdatedBow>();
        _playerCameraController = GetComponent<PlayerCameraController>();
        _meleeController = GetComponent<MeleeController>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _playerChecks.SetDrag();
        
        // if(!_aimingInput)
        //     _playerMovement.Rotate();
        
        if (_playerChecks.IsGrounded())
        {
            _playerMovement.Sprint(_sprintInput);
            _playerMovement.Crouch(_crouchInput);
            _playerCameraController.SetCrouch(_crouchInput);
            _playerMovement.Jump(_jumpInput);
            _playerMovement.Aim(_aimingInput);
        }

        if (_cameraInput.magnitude > 0.001f)
        {
            _playerCameraController.SetCameraRotation(_cameraInput);
        }

        if (_moveInput.magnitude > 0.01f)
        {
            _playerMovement.AirControl(_playerChecks.IsGrounded());
            if (!_aimingInput)
            {
                _playerMovement.Rotate(_moveInput);
                _playerMovement.Move(_playerChecks.CheckSlope(), _playerChecks.groundHitInfo.normal);
            }
            else
                _playerMovement.Strafe(_playerChecks.CheckSlope(), _playerChecks.groundHitInfo.normal, _moveInput);
        }
        
        _playerMovement.Fall(_playerChecks.IsGrounded());
        _playerBow.Aim(_aimingInput);
        if (_aimingInput)
        {
            var projectedPlane = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            Vector2 rotation = new Vector2(projectedPlane.x, projectedPlane.z);
            _playerMovement.RotateForAim(rotation);
        }
    }

    private void Update()
    {
        Events();
        Animation();
    }

    private void Animation()
    {
        _playerAnimations.MoveAnimation();
        _playerAnimations.DirectionAnimation(_moveInput.x);
        _playerAnimations.CrouchAnimation(_crouchInput);
        if (_playerChecks.IsGrounded() && _jumpInput)
        {
            _playerAnimations.JumpAnimation();
        }

        if (_playerChecks.IsGrounded())
        {
            _playerAnimations.StrafeAnimation(_aimingInput);
        }
    }

    #region Events

        private void Events()
        {
            if (_playerChecks.IsGrounded() && _sprintInput)
                OnSprintEvent.Invoke();
            if (_playerChecks.IsGrounded() && _crouchInput)
                OnCrouchEvent.Invoke();
            if (_playerChecks.IsGrounded() && _jumpInput)
                OnJumpEvent.Invoke();
            if(_playerChecks.IsLanding())
                OnLandEvent.Invoke();
        }

    #endregion

    #region Inputs
        public void GetCamera(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
            // _playerMovement.CameraControl(_cameraInput);
        }
        public void GetMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        public void GetAim(InputAction.CallbackContext context)
        {
            _aimingInput = context.action.WasPerformedThisFrame(); 
        }
        public void GetFire(InputAction.CallbackContext context)
        {
            // On fire down
            if (context.performed)
                _playerBow.Fire();    
        }
        public void GetMelee(InputAction.CallbackContext context)
        {
            // On melee down
            if (context.started)
                _meleeController.Melee();
        }
        public void GetCycleArrow(InputAction.CallbackContext context)
        {
            // On cycle arrow down
            if (context.started)
            {
                if (!_aimingInput)
                    _playerBow.CycleArrow();
            }
        }
        public void GetSprint(InputAction.CallbackContext context)
        {
            _sprintInput = context.action.WasPerformedThisFrame(); 
        }
        public void GetCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("performed");
                if (_crouchInput)
                    _crouchInput = false;
                else
                    _crouchInput = true;
            }
            // _crouchInput = context.action.WasPerformedThisFrame(); 
        }
        public void GetJump(InputAction.CallbackContext context)
        {
            _jumpInput = context.action.WasPerformedThisFrame(); 
        }
        public void GetUse(InputAction.CallbackContext context)
        {
            // On melee down
            if (context.started)
                _player.UseObject();
        }
    #endregion

    private void OnDrawGizmos()
    {
        
    }
}
