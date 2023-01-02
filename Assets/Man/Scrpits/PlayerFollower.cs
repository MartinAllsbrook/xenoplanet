using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform mainCameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
        transform.rotation = Quaternion.Euler(0, mainCameraTransform.rotation.eulerAngles.y, 0);
    }
}
