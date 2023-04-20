using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private Player _player;

    [Header("Dashing")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    // [SerializeField] private float _dashDuration;
    
    [Header("Cooldown")]
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashCoolDownTimer;
    private bool _isDashing;

    [Header("Reference")]
    [SerializeField] private MeshTrail _meshTrail;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private IEnumerator _meshTrailCoroutine;

    private float startFOV;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        
        startFOV = _virtualCamera.m_Lens.FieldOfView;
        _meshTrailCoroutine = _meshTrail.ActivateTrail(0.4f);
    }

    private void Update()
    {
        if (_dashCoolDownTimer > 0)
            _dashCoolDownTimer -= Time.deltaTime;  
    }

    public void Dash(bool Input, CinemachineVirtualCamera camera)
    {
        if (Input)
        {
            if (_dashCoolDownTimer > 0 || !_player.ChangeIntuition(-5)) 
                return;
            
            
            _dashCoolDownTimer = _dashCooldown;

            _isDashing = true;
            
            Vector3 calcDashForce = transform.forward * _dashForce + transform.up * _dashUpwardForce;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            _rigidbody.AddForce(calcDashForce * 100, ForceMode.Acceleration);

            StartCoroutine(_meshTrailCoroutine);
            DashEffects();
            
            Invoke("ResetDash", 0.4f);
        }
    }

    private void ResetDash()
    {
        _isDashing = false;
        StopCoroutine(_meshTrailCoroutine);
        DashEffectsReset();
    }
    
    private void DashEffectsReset()
    {
        _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp( _virtualCamera.m_Lens.FieldOfView, startFOV, 1f);
    }

    private void DashEffects()
    {
        _virtualCamera.m_Lens.FieldOfView = 65f;
    }
}
