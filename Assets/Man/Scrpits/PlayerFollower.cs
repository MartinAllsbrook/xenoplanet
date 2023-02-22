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
    public bool isAiming = false;
    private float _offset;

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
    void LateUpdate()
    {
        transform.position = playerTransform.position;
        transform.rotation = Quaternion.Euler(0, mainCameraTransform.rotation.eulerAngles.y, 0);
        cameraLookAt.position = transform.position + transform.up * 1.64f + transform.right * _offset;
        cameraFollow.position = cameraLookAt.position + Vector3.down * 1.64f;
        
    }

    IEnumerator Aim(float toFOV, float toOffset, float duration)
    {

        float counter = 0;

        float fromFOV = thirdPersonCamera.m_Lens.FieldOfView;
        var currentTransform = transform;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float lerpProgress = counter / duration;
            _offset = Mathf.Lerp(_offset, toOffset, lerpProgress);
            thirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(fromFOV, toFOV, lerpProgress);
            yield return null;
        }

    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            isAiming = true;
            if (_aimCoroutine != null)
                StopCoroutine(_aimCoroutine);
            _aimCoroutine = StartCoroutine(Aim(15f, cameraLookAtOffset, aimLerpDuration));
            crossHairController.ShowCrossHair();
            thirdPersonCamera.m_XAxis.m_MaxSpeed = xSensitivity / aimSensitivityMultiplier;
            thirdPersonCamera.m_YAxis.m_MaxSpeed = ySensitivity / aimSensitivityMultiplier;
        }
        else if (context.action.WasReleasedThisFrame())
        {
            isAiming = false;
            if (_aimCoroutine != null)
                StopCoroutine(_aimCoroutine);
            _aimCoroutine = StartCoroutine(Aim(40f, 0f, aimLerpDuration));
            crossHairController.HideCrossHair();
            thirdPersonCamera.m_XAxis.m_MaxSpeed = xSensitivity;
            thirdPersonCamera.m_YAxis.m_MaxSpeed = ySensitivity;
        }
    }
}
