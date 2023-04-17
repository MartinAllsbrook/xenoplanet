using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerUpdatedAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody _rigidbody;
    
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Crouch = Animator.StringToHash("Crouch");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Strafe = Animator.StringToHash("Strafe");
    private static readonly int X = Animator.StringToHash("xDir");
    private static readonly int Y = Animator.StringToHash("yDir");

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DirectionAnimation(Vector2 dir)
    {
        animator.SetFloat(X, dir.x);
        animator.SetFloat(Y, dir.y);
    }

    public void MoveAnimation()
    {
        animator.SetFloat(Speed, new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z).magnitude);
    }
    public void CrouchAnimation(bool isCrouched)
    {
        animator.SetBool(Crouch, isCrouched);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger(Jump);
    }

    public void StrafeAnimation(bool isAiming)
    {
        animator.SetBool(Strafe, isAiming);
    }
    
    
}
