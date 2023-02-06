using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollower : MonoBehaviour
{
    // public static PlayerFollower Instance;

    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private float cameraLookAtOffset;
    [SerializeField] private CrosshaireController crossHairController;
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

    private void OffsetCameraLook()
    {
        cameraLookAt.position = transform.position + transform.right * cameraLookAtOffset + transform.up * 1.64f;
    }

    private void ResetCameraLook()
    {
        cameraLookAt.position = transform.position + transform.up * 1.64f;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            OffsetCameraLook();
            crossHairController.ShowCrossHair();
        }
        else if (context.action.WasReleasedThisFrame())
        {
            ResetCameraLook();
            crossHairController.HideCrossHair();
        }
    }
}
