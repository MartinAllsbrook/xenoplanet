using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollower : MonoBehaviour
{
    // public static PlayerFollower Instance;
    [Range(0, 5)] [SerializeField] private float aimSensitivityMultiplier;
    [Range(0, 1)] [SerializeField] private float aimLerpDuration;
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private float cameraLookAtOffset;
    [SerializeField] private HUDController crossHairController;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    private Transform playerTransform;
    private Transform mainCameraTransform;
    private Coroutine _aimCoroutine;

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

    IEnumerator Aim(float toFOV, float offset, float duration)
    {
        
        float counter = 0;

        float fromFOV = thirdPersonCamera.m_Lens.FieldOfView;
        var currentTransform = transform;

        while (counter < duration)
        {
            var targetPosition = currentTransform.position + currentTransform.up * 1.64f + currentTransform.right * offset;
            counter += Time.deltaTime;
            float lerpProgress = counter / duration;
            cameraLookAt.position = Vector3.Lerp(cameraLookAt.position, targetPosition, lerpProgress);
            cameraFollow.position = cameraLookAt.position + Vector3.down * 1.64f;
            thirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(fromFOV, toFOV, lerpProgress);
            yield return null;
        }

    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if (_aimCoroutine != null)
                StopCoroutine(_aimCoroutine);
            _aimCoroutine = StartCoroutine(Aim(19f, cameraLookAtOffset, aimLerpDuration));
            crossHairController.ShowCrossHair();
            thirdPersonCamera.m_XAxis.m_MaxSpeed = xSensitivity / aimSensitivityMultiplier;
            thirdPersonCamera.m_YAxis.m_MaxSpeed = ySensitivity / aimSensitivityMultiplier;
        }
        else if (context.action.WasReleasedThisFrame())
        {
            if (_aimCoroutine != null)
                StopCoroutine(_aimCoroutine);
            _aimCoroutine = StartCoroutine(Aim(40f, 0f, aimLerpDuration));
            crossHairController.HideCrossHair();
            thirdPersonCamera.m_XAxis.m_MaxSpeed = xSensitivity;
            thirdPersonCamera.m_YAxis.m_MaxSpeed = ySensitivity;
        }
    }
}
