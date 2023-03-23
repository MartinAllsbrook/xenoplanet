using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdatedMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [Range(0,50)] [SerializeField] private float movementSpeed;
    [Range(0,2)] [SerializeField] private float sprintMultiplier;
    [Range(0,1)] [SerializeField] private float crouchMultiplier;
    [Range(0, 1)] [SerializeField] private float aimMultiplier;
    [Range(0,1)] [SerializeField] private float airControlMultiplier;
    [Range(0,100)] [SerializeField] private float jumpForce;
    [Range(0,50)] [SerializeField] private float fallForce;
    
    
    //Script References
    private PlayerUpdatedController _playerUpdatedController;
    private Rigidbody _rigidbody;
    
    //movement variables
    private float _angle;
    private float _targetAngle;
    private float _calcMoveSpeed;
    private Vector3 _camDirection;
    private float _turnVelocity;
    private Vector3 _currVelocity;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _playerUpdatedController = GetComponent<PlayerUpdatedController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(bool onSlope, Vector3 groundNormal)
    {
        if (onSlope)
            _camDirection = Vector3.ProjectOnPlane(_camDirection, groundNormal).normalized;
        
        //Actual Move
        _rigidbody.AddForce(_camDirection.normalized * (_calcMoveSpeed * 100), ForceMode.Force);
    }

    public void Strafe(bool onSlope, Vector3 groundNormal, Vector2 input)
    {
        if (onSlope)
            _camDirection = Vector3.ProjectOnPlane(_camDirection, groundNormal).normalized;
        
        //Actual Move
        Vector3 moveDirection = input.y * transform.forward + input.x * transform.right;
        _rigidbody.AddForce(moveDirection * (_calcMoveSpeed * 100), ForceMode.Force);
    }

    public void Aim(bool input)
    {
        if(input)
            _calcMoveSpeed = movementSpeed * aimMultiplier;
    }

    public void Sprint(bool input)
    {
        if(input)
            _calcMoveSpeed = movementSpeed * sprintMultiplier;
        else
            _calcMoveSpeed = movementSpeed;
    }
    public void Crouch(bool input)
    {
        if (input)
            _calcMoveSpeed = movementSpeed * crouchMultiplier;

        //Add code for colider!
    }
    public void AirControl(bool isGrounded)
    {
        if (!isGrounded)
        {
            _calcMoveSpeed = movementSpeed * airControlMultiplier;
        }
    }

    public void Fall(bool isGrounded)
    {
        if (_rigidbody.velocity.y < 2f && !isGrounded)
        {
            _rigidbody.AddForce(-Vector3.up * (fallForce * 10), ForceMode.Impulse);
        }
    }
    
    
    public void Rotate(Vector2 direction)
    {
        _targetAngle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _playerUpdatedController.mainCamera.transform.eulerAngles.y);
        
        _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnVelocity, 0.1f);

        _camDirection = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward;
        transform.rotation = Quaternion.Euler(0f, _angle, 0f);
    }

    public void RotateForAim(Vector2 direction)
    {
        Vector3 lookTowards = new Vector3(direction.x, 0, direction.y);

        Quaternion lookRotation = Quaternion.LookRotation(lookTowards);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 5);
        
        _camDirection = newRotation * Vector3.forward;
        transform.rotation = newRotation;
    }
    
    public void Jump(bool input)
    {
        if (input)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            _rigidbody.AddForce(Vector3.up * (jumpForce * 10), ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Debug.Log(_camDirection.normalized);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
    
    
}
