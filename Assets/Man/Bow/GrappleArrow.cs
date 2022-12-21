using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArrow : Arrow
{
    private LineRenderer ropeLineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ropeLineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] postitions = new Vector3[2];
        // Set postitions 0 => arrow, 1 => player
        postitions[0] = transform.position;
        postitions[1] = PlayerActions.Instance.transform.position + new Vector3(0, 1.5f, 0);
        // Render rope between player and arrow
        ropeLineRenderer.SetPositions(postitions);
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayerActions.Instance.Hooked = true;
        PlayerActions.Instance.HookPosition = transform.position;
        arrowRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
