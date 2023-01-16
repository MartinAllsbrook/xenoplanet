using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private LineRenderer laserLineRenderer;
    private Rigidbody laserRigidbody;

    private void Awake()
    {
        laserLineRenderer = GetComponent<LineRenderer>();
        laserRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        laserRigidbody.MovePosition(transform.position + transform.forward * speed);
        // var forward = transform.forward * 0.5f;
        // laserLineRenderer.SetPosition(0, -forward);
        // laserLineRenderer.SetPosition(1, forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
