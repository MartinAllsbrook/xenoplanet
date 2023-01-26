using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grapple : MonoBehaviour
{
    [SerializeField] private GameObject grappleProjectile;
    [Range(0,1)] [SerializeField] private float initialGrappleLength;
    [SerializeField] private float grappleRateOfChange;
    
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

    public void FireGrapple()
    {
        Vector3 direction = Bow.Instance.CalculateAimPosition(transform.position);
        Debug.Log("fire grapple");
        Instantiate(grappleProjectile, transform.position, Quaternion.LookRotation(direction));
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

        _joint.maxDistance = distanceFromPoint * initialGrappleLength;
        _joint.minDistance = 0;

        _joint.tolerance = 5f;
        _joint.spring = 400f;
        _joint.damper = 400f;
        _joint.massScale = 1f;

        _ropeRenderer.positionCount = 2;
    }

    private void Update()
    {
        ShrinkGrapple();
    }

    // Pull on grapple
    public void ShrinkGrapple()
    {
        if (_joint)
        {
            var grappleLength = _joint.maxDistance;

            if (grappleLength >= 0) 
                grappleLength -= Time.deltaTime * grappleRateOfChange;

            _joint.maxDistance = grappleLength;
        }
        
    }

    public void Unhook()
    {
        _ropeRenderer.positionCount = 0;
        if (_joint) 
            Destroy(_joint);
        
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
