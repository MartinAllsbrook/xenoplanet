using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerUpdatedChecks : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private float _maxSlopeAngle;
    [HideInInspector] public RaycastHit groundHitInfo;

    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGrounded()
    {
        if (Physics.CheckSphere(_groundCheck.position, 0.3f, WhatIsGround))
        {
            return true;
        }

        return false;
    }

    public bool IsLanding()
    {
        if (IsGrounded())
        {
            if (_rigidbody.velocity.y < -0.1f)
            {
                return true;
            }
        }
        
        return false;
    }

    public void SetDrag()
    {
        if (IsGrounded())
            _rigidbody.drag = 5;
        else
            _rigidbody.drag = 1;
    }
    
    public bool CheckSlope()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundHitInfo, Mathf.Infinity,
                WhatIsGround) && IsGrounded())
        {
            float curSlopeAngle = Vector3.Angle(groundHitInfo.normal, Vector3.up);
            return curSlopeAngle < _maxSlopeAngle && curSlopeAngle != 0;
        }

        return false;
    }
    
    private void CheckGravity()
    {
        _rigidbody.useGravity = !CheckSlope();
    }
}
