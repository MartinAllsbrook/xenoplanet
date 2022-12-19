using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody arrowRigidbody;

    public void Fire(float strength)
    {
        arrowRigidbody.AddForce(strength * 5000 * transform.forward);
        transform.LookAt(transform.position + arrowRigidbody.velocity.normalized);
    }
}
