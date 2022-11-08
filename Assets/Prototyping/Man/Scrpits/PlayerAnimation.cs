using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Components
    [SerializeField] private Animator _animation;
    private Rigidbody _rigidbody;
    private Material _material;

    //Animator Parameters
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Material>();
    }


    private void Update()
    {
       _animation.SetFloat(XVelocity, _rigidbody.velocity.x);
        // _animation.SetFloat(YVelocity, _rigidbody.velocity.y);
        
    }
    
    private IEnumerator SpawnAnimation()
    {
        _material.SetFloat("_Dissolve", 1);
        yield return new WaitForSeconds(0.1f);
    }
}
