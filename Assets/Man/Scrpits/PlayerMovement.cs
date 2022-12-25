using System;
using System.Collections;
using Cinemachine;
using Cinemachine.Utility;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    private Rigidbody _rigidbody;

    //Move smooth
    private Vector3 currVelocity;
    private float turnVelocity;

    //Move Factors
    [Range(1, 100)] [SerializeField] private float PlayerBaseSpeed;
    [Range(0, 2)] [SerializeField] private float PlayerSprintMultiplier;
    [Range(0, 2)] [SerializeField] private float PlayerCrouchMultiplier;
    [Range(0, 20)] [SerializeField] private float PlayerJumpForce;
    [Range(0, 4)] [SerializeField] private float doubbleJumpMultiplier;
    [Range(0, 100)] [SerializeField] private float PlayerFallForce;
    [SerializeField] private bool SecondJump;
    private Vector3 PlayerDirection;
    private bool PlayerJump;
    private bool PlayerSprint;
    private bool PlayerCrouch;
    

    // [Range(1, 100)] [SerializeField] private float maxVelocity;
    // [Range(1, 5000)] [SerializeField] private float stationaryDrag;
    
    //Camera Reference
    [SerializeField] private CinemachineFreeLook thridPersonCamera;
    [SerializeField] private Camera mainCamera;
    
    //Checks
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    
    public bool isJump;

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
        // Debug.Log(isGrounded);
    }

    public void PlayerInput(Vector3 PlayerDirInput, bool PlayerJumpInput, bool PlayerSprintInput, bool PlayerCrouchInput)
    {
        PlayerDirection = PlayerDirInput;
        PlayerJump = PlayerJumpInput;
        PlayerSprint = PlayerSprintInput;
        PlayerCrouch = PlayerCrouchInput;
        // Debug.Log(PlayerSprint);
    }

    #region Movment
    public void Jump()
    {
        // If the player jumped this update
        if (PlayerJump)
        {
            // If the player is on the ground jump
            if (isGrounded)
                _rigidbody.velocity += Vector3.up * PlayerJumpForce;

            // If the player is in the air
            else
            {
                // If the player has a 2nd jump double jump
                if (SecondJump)
                {
                    //without this double jump on falling is too small
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z); // zero out velocity before jumping
                    // _rigidbody.velocity += Vector3.up * (PlayerJumpForce * Time.deltaTime * 50);
                    // float temp = (PlayerJumpForce * 20f);
                    // _rigidbody.velocity += new Vector3(PlayerDirection.x, temp, PlayerDirection.y);
                    _rigidbody.velocity += Vector3.up * (PlayerJumpForce * doubbleJumpMultiplier);
                    Debug.Log("DoubbleJump");
                    SecondJump = false;
                }
                // Else fall faster
                else
                    _rigidbody.velocity += Vector3.down * (PlayerFallForce * Time.deltaTime);
            }
        }
    }

    public void Move()
    {
        if (isGrounded)
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

        //AirControl??
        if (!isGrounded)
        {
            
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
            isGrounded = true;
            SecondJump = true;
        }
        else
            isGrounded = false;
    }
    

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
