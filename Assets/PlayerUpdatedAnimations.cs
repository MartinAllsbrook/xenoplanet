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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
    
    
}
