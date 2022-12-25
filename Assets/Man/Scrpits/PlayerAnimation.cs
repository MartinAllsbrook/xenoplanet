using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerMovement _playerMovement;
    
    //Components
    [SerializeField] private Animator _animation;
    private Rigidbody _rigidbody;
    private Material _material;

    //Animator Parameters
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Material>();
    }

    private void Update()
    {
       _animation.SetFloat(XVelocity, new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude);
       _animation.SetFloat(YVelocity, _rigidbody.velocity.y);
       _animation.SetBool(IsGrounded, _playerMovement.isGrounded);
       
       // Debug.Log("poop" + _playerMovement.isGrounded);   

    }
    
}
