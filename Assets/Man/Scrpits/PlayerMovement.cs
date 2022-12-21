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
    [Range(0, 100)] [SerializeField] private float PlayerJumpForce;
    [Range(0, 100)] [SerializeField] private float PlayerFallForce;
    [SerializeField] private bool SecondJump;
    private Vector3 PlayerDirection;
    private float PlayerJump;

    // [Range(1, 100)] [SerializeField] private float maxVelocity;
    // [Range(1, 5000)] [SerializeField] private float stationaryDrag;
    
    //Camera Reference
    [SerializeField] private CinemachineFreeLook thridPersonCamera;
    [SerializeField] private Camera mainCamera;
    
    //Checks
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    
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
    }

    public void PlayerInput(Vector3 PlayerDirInput, float PlayerJumpInput)
    {
        PlayerDirection = PlayerDirInput;
        PlayerJump = PlayerJumpInput;
    }
    
    
    
    
    #region Movment
    public void Jump()
        {
            //if you can jump (isgrounded and input)
            if (isGrounded && PlayerJump == 1)
            {
                //jump
                _rigidbody.velocity += Vector3.up * (PlayerJumpForce);
            }
            
            //if falling
            if (!isGrounded && _rigidbody.velocity.y < 0)
            {
                //can second jump
                if (SecondJump && PlayerJump == 1)
                {
                    //zero out velocity before jumping
                    _rigidbody.velocity = Vector3.zero;
                    // _rigidbody.velocity += Vector3.up * (PlayerJumpForce * Time.deltaTime * 50);
                    float temp = (PlayerJumpForce * 20f);
                    _rigidbody.velocity += new Vector3(PlayerDirection.x, temp, PlayerDirection.y);
                    SecondJump = false;
                }
                
                else
                {
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
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
