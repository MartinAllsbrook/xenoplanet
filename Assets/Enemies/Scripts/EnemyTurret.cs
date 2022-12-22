using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    private GameObject cannon;
    private LineRenderer _laserBeam;

    private void Awake()
    {
        cannon = transform.GetChild(0).gameObject;
        _laserBeam = cannon.GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Bow.Instance.transform.position + new Vector3(0,1,0) - transform.position);
        if (Physics.Raycast(ray, out hit, range, visible))
        {
            Debug.Log(hit.transform.gameObject.tag);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                FireLaser();
            }
        }
    }

    private void FireLaser()
    {
        Debug.Log("Can see player");
        cannon.transform.LookAt(Bow.Instance.transform.position);
        Vector3[] postitions = new Vector3[2];
        postitions[0] = transform.position;
        postitions[1] = Bow.Instance.transform.position + new Vector3(0, 1.5f, 0);
        _laserBeam.SetPositions(postitions);
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, Bow.Instance.transform.position - transform.position);
        Gizmos.DrawRay(ray);    
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
