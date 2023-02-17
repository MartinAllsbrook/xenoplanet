using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Bow : MonoBehaviour
{
    public static Bow Instance;
    
    [SerializeField] private GameObject[] arrows;
    // [SerializeField] private Transform arrowSpawnPosition;
    [SerializeField] private LayerMask aimingLayerMask;
    [Range(0, 5)] [SerializeField] private float aimSensitivityMultiplier;
    [Range(0, 1)] [SerializeField] private float aimLerpDuration;
    [SerializeField] private Vector3 aimOffset;
    [SerializeField] private float maxAimDistance;
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeDamage;
    [SerializeField] private PlayerFollower playerFollower;
    [SerializeField] private float cameraLookAtOffset;
    [SerializeField] private HUDController crossHairController;
    [SerializeField] private float cameraTurn;
    
    private GameObject mainCamera;
    private CinemachineFreeLook thirdPersonCamera;
    private Transform arrowAimer;
    private CinemachineImpulseSource ImpulseSource;
    
    private float chargeTime = 0;
    private float strength = 0;
    private int selectedArrow = 0;
    private float chargeMultiplier = 3;
    private ParticleSystem meleeParticleSystem;
    private bool _chargingInput = false;
    private bool _aimingInput = false;

    public bool isAiming;

    private bool _aimingCoroutinesRunning;

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        thirdPersonCamera = FindObjectOfType<CinemachineFreeLook>();
        if(!thirdPersonCamera.CompareTag("MainCamera"))
            Debug.LogError("Cannot find thirdPersonCamera");
        
        var cameras = FindObjectsOfType<Camera>();
        foreach (var camera in cameras)
        {
            if (camera.CompareTag("MainCamera"))
                mainCamera = camera.gameObject;
        }
        if(!mainCamera.CompareTag("MainCamera"))
            Debug.LogError("Cannot find mainCamera");

        arrowAimer = mainCamera.transform.GetChild(1);
        if (!arrowAimer.CompareTag("MainCamera"))
            Debug.LogError("Cannot find arrowAimer");

        meleeParticleSystem = GetComponent<ParticleSystem>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        
        thirdPersonCamera.m_XAxis.m_MaxSpeed = 100;
        thirdPersonCamera.m_YAxis.m_MaxSpeed = 1;
    }

    private void Update()
    {
        if (_chargingInput)
        {
            ChargeArrow();
        }

        // if (_aimingInput)
        // {
        //     AimArrow();
        // }
        // else
        // {
        //     ResetAim();
        // }
    }

    #region GetInputs

    public void GetChargingInput(InputAction.CallbackContext context)
    {
        _chargingInput = context.action.WasPerformedThisFrame();
        if (context.action.WasReleasedThisFrame())
        {
            FireArrow();
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            AimArrow();
        }
        else if (context.action.WasReleasedThisFrame())
        {
            ResetAim();
        }
    }

    public void GetMeleeInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            Melee();
        }
    }
    
    public void GetCycleInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            CycleArrow();
        }
    }

    #endregion

    private void CycleArrow()
    {
        // Cycle through arrows
        selectedArrow++;
        if (selectedArrow >= arrows.Length)
            selectedArrow = 0;
        
        // Update hud
        HUDController.Instance.SetArrow(arrows[selectedArrow].GetComponent<Arrow>().ArrowName);
    }
    
    // "Charge" arrow based on how long the player is holding the trigger
    private void ChargeArrow()
    {
        // The longer the player holds the trigger the slower strength builds up
        chargeTime += Time.deltaTime * chargeMultiplier;
        strength = Mathf.Pow(chargeTime, 1/chargeMultiplier);
        
        // Change fov to match arrow charge
        // thirdPersonCamera.m_Lens.FieldOfView = strength * 4 + 45;
        // ChargeZoom();
        
    }

    private void AimArrow()
    {
        isAiming = true;
        
        
        StartCoroutine(lerpFieldOfView(thirdPersonCamera, 19f, aimLerpDuration));
        StartCoroutine(playerFollower.LerpCameraOffset(cameraLookAtOffset, aimLerpDuration));
        crossHairController.ShowCrossHair();
        
        //Sensitivity
        thirdPersonCamera.m_XAxis.m_MaxSpeed /= aimSensitivityMultiplier;
        thirdPersonCamera.m_YAxis.m_MaxSpeed /= aimSensitivityMultiplier;
        
        // thirdPersonCamera.m_
        // StartCoroutine(lerpFieldOfView(aimOffset, aimLerpDuration));
    }

    private void ResetAim()
    {
        Debug.Log("release");
        StartCoroutine(lerpFieldOfView(thirdPersonCamera, 34f, aimLerpDuration));
        StartCoroutine(playerFollower.LerpCameraOffset(0, aimLerpDuration));
        crossHairController.HideCrossHair();
        
        thirdPersonCamera.m_XAxis.m_MaxSpeed *= aimSensitivityMultiplier;
        thirdPersonCamera.m_YAxis.m_MaxSpeed *= aimSensitivityMultiplier;

        //hard coded value
        Vector3 returnPos = new Vector3(0, 1.64f, 0);
        
        isAiming = false;
    }
    
    
    IEnumerator lerpFieldOfView(CinemachineFreeLook targetCamera, float toFOV, float duration)
    {
        _aimingCoroutinesRunning = true;
        
        float counter = 0;

        float fromFOV = targetCamera.m_Lens.FieldOfView;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            float fOVTime = counter / duration;
            Debug.Log(fOVTime);

            //Change FOV
            
            // thirdPersonCamera.m_XAxis.m_InputAxisValue = cameraTurn;
            targetCamera.m_Lens.FieldOfView = Mathf.Lerp(fromFOV, toFOV, fOVTime);
            //Wait for a frame
            yield return null;
        }

        _aimingCoroutinesRunning = false;
    }

    private void FireArrow()
    {
        Vector3 spawnPosition = transform.position;
        Vector3 arrowDirection = CalculateAimPosition(spawnPosition);

        var arrowInstance = Instantiate(arrows[selectedArrow], spawnPosition, Quaternion.LookRotation(arrowDirection));
            
        // Add force to the arrow equal to strength
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        
        // Reset FOV and vars
        // thirdPersonCamera.m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
        
        //Impulse
        ImpulseSource.GenerateImpulse();
    }
    
    private void Melee()
    {
        Debug.Log("Melee");
        meleeParticleSystem.Play();
        Ray ray = CreateRayFromCamera();
        if (Physics.Raycast(ray, out RaycastHit hit, 20, aimingLayerMask))
        {
            if ((hit.point - transform.position).magnitude > meleeDistance)
                return;

            if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Breakable Environment"))
                hit.transform.gameObject.GetComponent<BreakableObject>().Health = -meleeDamage;
            
        }
            
    }
    
    public Vector3 CalculateAimPosition(Vector3 spawnPosition)
    {
        Ray ray = CreateRayFromCamera();
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimingLayerMask))
            arrowAimer.position = hit.point;
        else
            arrowAimer.position = mainCamera.transform.position + mainCamera.transform.forward * maxAimDistance;

        return arrowAimer.position - spawnPosition;
    }
    
    private Ray CreateRayFromCamera()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;
        return new Ray(origin, direction);
        
    }
}