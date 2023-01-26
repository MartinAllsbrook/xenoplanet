using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    // public static PlayerFollower Instance;
    
    private Transform playerTransform;
    private Transform mainCameraTransform;

    private void Awake()
    {
        // if (Instance == null)
        //     Instance = this;
    }

    void Start()
    {
        playerTransform = Player.Instance.transform;
        var cameras = FindObjectsOfType<Camera>();
        foreach (var camera in cameras)
        {
            if (camera.CompareTag("MainCamera"))
                mainCameraTransform = camera.transform;
        }
        if(!mainCameraTransform.CompareTag("MainCamera"))
            Debug.LogError("Cannot find mainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
        transform.rotation = Quaternion.Euler(0, mainCameraTransform.rotation.eulerAngles.y, 0);
    }
}
