using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody arrowRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Vector3.forward);
        // arrowRigidbody.velocity = Vector3.forward * 10;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(arrowRigidbody.velocity);
    }
}
