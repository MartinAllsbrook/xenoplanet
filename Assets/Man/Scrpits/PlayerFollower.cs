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
    [SerializeField] private HUDController crossHairController;
    private Transform playerTransform;
    private Transform mainCameraTransform;
    private Coroutine offsetRoutine;
    private Coroutine resetRoutine;

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

    private IEnumerator OffsetCameraLook()
    {
        var targetPosition = transform.position + transform.right * cameraLookAtOffset + transform.up * 1.64f;
        while ((cameraLookAt.position - targetPosition).magnitude > 0.1f)
        {
            targetPosition = transform.position + transform.right * cameraLookAtOffset + transform.up * 1.64f;
            cameraLookAt.position = Vector3.Lerp(cameraLookAt.position, targetPosition, 0.1f);
            
            yield return null;
        }
        yield return null;
    }

    private IEnumerator ResetCameraLook()
    {
        var targetPosition = transform.position + transform.up * 1.64f;
        while ((cameraLookAt.position - targetPosition).magnitude > 0.1f)
        {
            targetPosition = transform.position + transform.up * 1.64f;
            cameraLookAt.position = Vector3.Lerp(cameraLookAt.position, targetPosition, 0.1f);
            yield return null;
        }
        yield return null;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if (resetRoutine != null)
                StopCoroutine(resetRoutine);
            offsetRoutine = StartCoroutine(OffsetCameraLook());
            crossHairController.ShowCrossHair();
        }
        else if (context.action.WasReleasedThisFrame())
        {
            if (offsetRoutine != null)
                StopCoroutine(offsetRoutine);
            resetRoutine = StartCoroutine(ResetCameraLook());
            crossHairController.HideCrossHair();
        }
    }
}
