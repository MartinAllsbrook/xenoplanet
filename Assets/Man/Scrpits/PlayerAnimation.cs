using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement _player;
    
    //Components
    [SerializeField] private Animator _animation;
    private Rigidbody _rigidbody;
    private Material _material;

    //Animator Parameters
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Material>();
    }

    private void Start()
    {
        SpawnAnimation();
    }

    private void Update()
    {
       _animation.SetFloat(XVelocity, _rigidbody.velocity.magnitude);
       // _animation.SetBool(IsJump, _player.isJump);
    }
    
    private IEnumerator SpawnAnimation()
    {
        _material.SetFloat("_Dissolve", 0.5f);
        yield return new WaitForSeconds(0.1f);
    }
}
