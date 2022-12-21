using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleArrow : Arrow
{
    private LineRenderer ropeLineRenderer;

    private UnityEvent unHook;
    // Start is called before the first frame update
    void Start()
    {
        // Find unhook event and subscribe
        unHook = Bow.Instance.unHook;
        unHook.AddListener(UnHook);
        ropeLineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set postitions 0 => arrow, 1 => player
        Vector3[] postitions = new Vector3[2];
        postitions[0] = transform.position;
        postitions[1] = Bow.Instance.transform.position + new Vector3(0, 1.5f, 0);
        
        // Render rope between player and arrow
        ropeLineRenderer.SetPositions(postitions);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Set bow as hooked;
        Bow.Instance.Hooked = true;
        Bow.Instance.HookPosition = transform.position;
        arrowRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnHook()
    {
        Debug.Log("Destroy arrow");
        Destroy(gameObject);
    }
}
