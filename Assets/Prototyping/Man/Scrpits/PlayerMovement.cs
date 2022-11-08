using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    private Rigidbody _rigidbody;

    //Move smooth
    private Vector3 currVelocity;
    private float turnVelocity;

    //Move Factors
    [Range(1, 10)] [SerializeField] private float PlayerSpeed;
    [Range(1, 10)] [SerializeField] private float PlayerSprintSpeed;
    [Range(1, 10)] [SerializeField] private float PlayerCrouchSpeed;
    [Range(1, 50)] [SerializeField] private float PlayerJumpForce;
    
    //Camera Reference
    [SerializeField] private CinemachineFreeLook thridPersonCamera;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void Jump()
    {
        Debug.Log("Jump");
        _rigidbody.velocity += Vector3.up * PlayerJumpForce;
    }

    public void Move(Vector2 direction, float sprint, float crouch)
    {
        if(direction.magnitude >= 0.1f)
        {
            //code adapted from Brackeys
            
            //Rotate
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f) ;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            //Move
            Vector3 camDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            //Move Type
            if(sprint == 0 && crouch == 0)
                _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity,  camDirection.normalized * PlayerSpeed, ref currVelocity, 0.1f);
            else if (sprint == 1)
                _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity,  camDirection.normalized * PlayerSprintSpeed, ref currVelocity, 0.1f);
            else if (crouch == 1)
            {
                _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity,  camDirection.normalized * PlayerCrouchSpeed, ref currVelocity, 0.1f);
            }
        }
        else
            _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity,  Vector3.zero, ref currVelocity, 0.1f);
    }

    public void CameraControl(Vector2 direction)
    {
        thridPersonCamera.m_XAxis.m_InputAxisValue = -direction.x;
        thridPersonCamera.m_YAxis.m_InputAxisValue = -direction.y;
    }

    
}
