using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Grounded : BaseState
{
    [SerializeField] private CharacterControllerFSM _fsmReference; // Reference to the main FSM
    [Range(1, 100)] [SerializeField] private float PlayerBaseSpeed;
    [Range(0, 2)] [SerializeField] private float PlayerSprintMultiplier;
    [Range(0, 2)] [SerializeField] private float PlayerCrouchMultiplier;
    [Range(0, 20)] [SerializeField] private float PlayerJumpForce;
    
    // Move smooth
    private Vector3 currVelocity;
    private float turnVelocity;
    
    // public Grounded(CharacterControllerFSM stateMachine) : base("Grounded", stateMachine) {
    //     _fsmReference = stateMachine;
    //     // Debug.Log("Entered idle state");
    // }
    
    public override void UpdateLogic()
    {
        Move();
        Jump();
    }
    
    private void Jump()
    {
         // If the player jumped this update, Dont need to check if the player is grounded because this can only be called if the player is grounded
         if (_fsmReference.JumpInput) // TODO: Decide where to put jump
             _fsmReference.playerRigidbody.velocity += Vector3.up * PlayerJumpForce; // Should change this to AddForce() 
    }

    private void Move()
    {
        //Calculate direction to move
        float targetAngle = Mathf.Atan2(_fsmReference.moveInput.x, _fsmReference.moveInput.y) * Mathf.Rad2Deg + _fsmReference.mainCamera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(_fsmReference.transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f);
    
        //Move
        Vector3 camDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    
        //if input then turn and move
        if (_fsmReference.moveInput.magnitude > 0.1f)
        {
            Debug.Log("hmmmm");
            //apply camera rotation
            _fsmReference.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
            if(_fsmReference.sprintInput)
                _fsmReference.playerRigidbody.velocity = Vector3.SmoothDamp(_fsmReference.playerRigidbody.velocity, camDirection * (PlayerBaseSpeed * PlayerSprintMultiplier * _fsmReference.moveInput.magnitude), ref currVelocity, 0.3f);
            if(_fsmReference.crouchInput)
                _fsmReference.playerRigidbody.velocity = Vector3.SmoothDamp(_fsmReference.playerRigidbody.velocity, camDirection * (PlayerBaseSpeed * PlayerCrouchMultiplier * _fsmReference.moveInput.magnitude), ref currVelocity, 0.3f);
            else
                _fsmReference.playerRigidbody.velocity = Vector3.SmoothDamp(_fsmReference.playerRigidbody.velocity, camDirection * (PlayerBaseSpeed * _fsmReference.moveInput.magnitude), ref currVelocity, 0.3f);
        }
        //if no input 0 velocity (prevents sliding)
        else
        {
            _fsmReference.playerRigidbody.velocity = Vector3.SmoothDamp(_fsmReference.playerRigidbody.velocity, Vector3.zero, ref currVelocity, 0.2f);
        }
    }
}
