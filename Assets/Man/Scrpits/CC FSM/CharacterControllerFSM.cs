using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharacterControllerFSM : StateMachine
{
    [SerializeField] public Grounded groundedState;
    
    // Components
    [HideInInspector] public Rigidbody playerRigidbody;
    
    [HideInInspector] public Vector3 moveInput;
    private bool _jumpInput;
    public bool JumpInput
    {
        get
        {
            if (_jumpInput)
            {
                var temp = _jumpInput;
                _jumpInput = false;
                return temp;
            }
            return _jumpInput;
        }
        set { _jumpInput = value; }
    }
    [HideInInspector] public bool sprintInput;
    [HideInInspector] public bool crouchInput;
    
    [HideInInspector] public bool PlayerSecondJump;
    
    public Camera mainCamera;

    private void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    protected override BaseState GetInitialState()
    {
        return groundedState;
    }
    
    // Public method to receive inputs from input controller
    public void PlayerInput(Vector3 PlayerDirInput, bool PlayerJumpInput, bool PlayerSprintInput, bool PlayerCrouchInput)
    {
        moveInput = PlayerDirInput;
        _jumpInput = PlayerJumpInput;
        sprintInput = PlayerSprintInput;
        crouchInput = PlayerCrouchInput;
    }
}
