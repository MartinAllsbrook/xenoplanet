using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource ambientAudio;
    [SerializeField] private float maxTimeAlive;

    private float _timeAlive;
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
        
        _timeAlive += Time.deltaTime;
        if (_timeAlive > maxTimeAlive)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.DealDamage(5);            
        }

        if (collision.gameObject.CompareTag("Breakable Environment"))
        {
            BreakableObject breakableObject = collision.gameObject.GetComponent<BreakableObject>();
            breakableObject.ChangeHealth(-20);
        }

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        
        speed = 0;
        Destroy(laserLineRenderer);
        Destroy(ambientAudio);
        hitAudio.Play();

        yield return new WaitForSeconds(4f);
        
        Destroy(gameObject);
    }
}
