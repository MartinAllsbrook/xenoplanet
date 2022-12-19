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
    [Range(1, 1000)] [SerializeField] private float PlayerJumpForce;
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
        
        CheckGrounded();
    }
    
    #region Movment
    
        public void Jump()
        {
            if (isGrounded)
            {
                _rigidbody.velocity += Vector3.up * PlayerJumpForce;
            }
            // isJump = true;
            // _rigidbody.AddForce(Vector3.up * PlayerJumpForce);
            // Debug.Log(Vector3.up * PlayerJumpForce);
        }
        
        /*public void Move(Vector2 direction, float sprint, float crouch)
        {
            if(direction.magnitude >= 0.1f)
            {
                // Code adapted from Brackeys 
        
                //Rotate
                float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f) ;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
                //Move
                Vector3 camDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
                // Calculate forces to add
                Debug.Log(_rigidbody.velocity.magnitude);
                Vector3 forceToAdd;
                var horizontalVelocity = _rigidbody.velocity.ProjectOntoPlane(new Vector3(0, 1, 0));
                var parallelVelocity = Vector3.Project(horizontalVelocity, camDirection);
        
                forceToAdd = camDirection.normalized * PlayerBaseSpeed *  (1 - parallelVelocity.magnitude/maxVelocity);
        
                //Move Type
                if (sprint == 0 && crouch == 0)
                {
                    
                }
                else if (sprint == 1)
                    forceToAdd *= PlayerSprintMultiplier;
                else if (crouch == 1)
                    forceToAdd *= PlayerCrouchMultiplier;
        
                _rigidbody.AddForce(forceToAdd);
        
            }
            else
            {
                _rigidbody.AddForce(-_rigidbody.velocity.normalized.ProjectOntoPlane(new Vector3(0, 1, 0)) * stationaryDrag);
            }
        }*/

        public void Move(Vector2 direction)
        {
            if (isGrounded)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f) ;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                //Move
                Vector3 camDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                if (direction.magnitude > 0.1f)
                {
                    _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, camDirection * (PlayerBaseSpeed * direction.magnitude), ref currVelocity, 0.2f);
                }
                else
                {
                    _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, Vector3.zero, ref currVelocity, 0.2f);
                }
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
        if (Physics.CheckSphere(groundCheck.position, 0.1f, WhatIsGround))
        {
            Debug.Log("walkable");
            isGrounded = true;
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
