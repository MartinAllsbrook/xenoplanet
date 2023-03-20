using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdatedMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [Range(0,50)] [SerializeField] private float _movementSpeed;
    [Range(0,2)] [SerializeField] private float _sprintMultiplier;
    [Range(0,1)] [SerializeField] private float _crouchMultiplier;
    [Range(0,1)] [SerializeField] private float _airControlMultiplier;
    [Range(0,100)] [SerializeField] private float _jumpForce;
    [Range(0,50)] [SerializeField] private float _fallForce;
    
    
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
    public void Sprint(bool input)
    {
        if(input)
            _calcMoveSpeed = _movementSpeed * _sprintMultiplier;
        else
            _calcMoveSpeed = _movementSpeed;
    }
    public void Crouch(bool input)
    {
        if (input)
            _calcMoveSpeed = _movementSpeed * _crouchMultiplier;

        //Add code for colider!
    }
    public void AirControl(bool isGrounded)
    {
        if (!isGrounded)
        {
            _calcMoveSpeed = _movementSpeed * _airControlMultiplier;
        }
    }

    public void Fall(bool isGrounded)
    {
        if (_rigidbody.velocity.y < 2f && !isGrounded)
        {
            _rigidbody.AddForce(-Vector3.up * (_fallForce * 10), ForceMode.Impulse);
        }
    }
    
    
    public void Rotate(Vector2 input)
    {
        _targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + _playerUpdatedController.mainCamera.transform.eulerAngles.y;
        _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnVelocity, 0.1f);

        _camDirection = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward;
        
        transform.rotation = Quaternion.Euler(0f, _angle, 0f);
    }
    
    
    
    public void Jump(bool input)
    {
        if (input)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            _rigidbody.AddForce(Vector3.up * (_jumpForce * 10), ForceMode.Impulse);
        }
    }
    

    public void CameraControl(Vector2 input)
    {
        _playerUpdatedController.thirdPersonCamera.m_XAxis.m_InputAxisValue = -input.x;
        _playerUpdatedController.thirdPersonCamera.m_YAxis.m_InputAxisValue = -input.y;
    }
    
    private void OnDrawGizmos()
    {
        Debug.Log(_camDirection.normalized);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
    
    
}
