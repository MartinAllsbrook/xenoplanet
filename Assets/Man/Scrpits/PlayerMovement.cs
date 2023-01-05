using System;
using System.Collections;
using Cinemachine;
using Cinemachine.Utility;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody _rigidbody;

    // Move smooth
    private Vector3 currVelocity;
    private float turnVelocity;

    // Move Factors
    [Header("Movement Speeds")]
    [Space(15)]
    [Range(1, 100)] [SerializeField] private float PlayerBaseSpeed;
    [Range(0, 2)] [SerializeField] private float PlayerSprintMultiplier;
    [Range(0, 2)] [SerializeField] private float PlayerCrouchMultiplier;
    [Range(0, 20)] [SerializeField] private float PlayerJumpForce;
    [Range(0, 4)] [SerializeField] private float doubbleJumpMultiplier;
    [Range(0, 100)] [SerializeField] private float PlayerFallForce;
    [SerializeField] private bool CanSecondJump;
    
    private Vector3 PlayerDirection;
    private bool PlayerJump;
    private bool PlayerSecondJump;
    private bool PlayerSprint;
    private bool PlayerCrouch;
    
    
    //Camera Reference
    [Header("Camera Reference")]
    [Space(15)]
    [SerializeField] private CinemachineFreeLook thridPersonCamera;
    [SerializeField] private Camera mainCamera;
    
    //Checks
    [Header("Player Checks")]
    [Space(15)]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundCheck;
    [HideInInspector]public bool isGrounded;
    private bool wasGrounded;
    private bool isJumping;
    
    [Header("Player Events")]
    [Space(15)]
    [SerializeField] UnityEvent OnJumpEvent;
    [SerializeField] UnityEvent OnLandEvent;
    [SerializeField] UnityEvent OnSprintEvent;
    [SerializeField] UnityEvent OnCrouchEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // Debug.Log(_rigidbody.velocity);
    }

    private void Update()
    {
        // Debug.Log(isJump);
        Move();
        Jump();
        
        CheckGrounded();
        
        Events();
    }

    public void PlayerInput(Vector3 PlayerDirInput, bool PlayerJumpInput, bool PlayerSprintInput, bool PlayerCrouchInput)
    {
        PlayerDirection = PlayerDirInput;
        PlayerJump = PlayerJumpInput;
        PlayerSprint = PlayerSprintInput;
        PlayerCrouch = PlayerCrouchInput;
    }

    #region Movment
    public void Jump()
    {
        // If the player jumped this update
        if (PlayerJump && isGrounded)
        {
            _rigidbody.velocity += Vector3.up * PlayerJumpForce;
        }

        //if not grounded and can second jump on update
        if (PlayerJump && PlayerSecondJump && CanSecondJump && !isGrounded)
        {
            //zero out y velocity before second jump
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z); 
            _rigidbody.velocity += Vector3.up * (PlayerJumpForce * doubbleJumpMultiplier);
            PlayerSecondJump = false;
        }

        //if falling
        if (_rigidbody.velocity.y < 0 && !isGrounded)
        {
            _rigidbody.velocity += Vector3.down * (PlayerFallForce * Time.deltaTime);
        }
    }

    public void Move()
    {
            //Calculate direction to move
            float targetAngle = Mathf.Atan2(PlayerDirection.x, PlayerDirection.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f);

            //Move
            Vector3 camDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //if input then turn and move
            if (PlayerDirection.magnitude > 0.1f)
            {
                //apply camera rotation
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                if(PlayerSprint)
                    _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, camDirection * (PlayerBaseSpeed * PlayerSprintMultiplier * PlayerDirection.magnitude), ref currVelocity, 0.3f);
                if(PlayerCrouch)
                    _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, camDirection * (PlayerBaseSpeed * PlayerCrouchMultiplier * PlayerDirection.magnitude), ref currVelocity, 0.3f);
                else
                    _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, camDirection * (PlayerBaseSpeed * PlayerDirection.magnitude), ref currVelocity, 0.3f);
            }
            //if no input 0 velocity (prevents sliding)
            else
            {
                _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, Vector3.zero, ref currVelocity, 0.2f);
            }
    }

    public void CameraControl(Vector2 direction)
    {
        thridPersonCamera.m_XAxis.m_InputAxisValue = -direction.x;
        thridPersonCamera.m_YAxis.m_InputAxisValue = -direction.y;
    }
        
    #endregion


    #region Checks

    private void CheckGrounded()
    {
        // If detector detects ground
        if (Physics.CheckSphere(groundCheck.position, 0.1f, WhatIsGround))
        {
            // Debug.Log("walkable");
            wasGrounded = isGrounded;
            isGrounded = true;
            PlayerSecondJump = true;
        }
        else
            isGrounded = false;
    }
    

    #endregion

    private void Events()
    {
        if(PlayerJump && isGrounded) 
            OnJumpEvent.Invoke();
        if(PlayerSecondJump && CanSecondJump && PlayerJump)
            OnJumpEvent.Invoke();
        if(PlayerSprint && isGrounded) 
            OnSprintEvent.Invoke();
        if(PlayerCrouch && isGrounded)
            OnCrouchEvent.Invoke();
        if(isGrounded && !wasGrounded)
            OnLandEvent.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
