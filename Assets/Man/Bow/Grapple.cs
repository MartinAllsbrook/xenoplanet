using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grapple : MonoBehaviour
{
    public static Grapple Instance;
    
    private GameObject _player;
    // private Rigidbody _playerRigidBody;
    private SpringJoint _joint;
    private LineRenderer _ropeRenderer;
    private Vector3 _grapplePoint;

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _player = Player.Instance.gameObject;
        // _playerRigidBody = _player.GetComponent<Rigidbody>();
        _ropeRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public void Hook(Vector3 point)
    {
        Unhook();
        
        _grapplePoint = point;
        
        _joint = _player.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _grapplePoint;

        float distanceFromPoint = Vector3.Distance(_player.transform.position, _grapplePoint);

        _joint.maxDistance = distanceFromPoint * 0.3f;
        _joint.minDistance = 0;

        _joint.spring = 30f;
        _joint.damper = 30f;
        _joint.massScale = 4.5f;

        _ropeRenderer.positionCount = 2;
    }

    // Pull on grapple
    public void Pull()
    {
        
    }

    public void Unhook()
    {
        _ropeRenderer.positionCount = 0;
        if (_joint) Destroy(_joint);
        
    }

    private void DrawRope()
    {
        // Dont draw rope if there is no grapple
        if (!_joint)
            return;
        
        // Set positions
        _ropeRenderer.SetPosition(0, transform.position);
        _ropeRenderer.SetPosition(1, _grapplePoint);
    }
}
