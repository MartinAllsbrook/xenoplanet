using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Bow : MonoBehaviour
{
    public static Bow Instance;
    
    [SerializeField] private GameObject[] arrows;
    // [SerializeField] private Transform arrowSpawnPosition;
    [SerializeField] private LayerMask aimingLayerMask;
    [SerializeField] private float maxAimDistance;
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeDamage;

    private GameObject mainCamera;
    private CinemachineFreeLook thirdPersonCamera;
    private Transform arrowAimer;
    
    private float chargeTime = 0;
    private float strength = 0;
    private int selectedArrow = 0;
    private float chargeMultiplier = 3;
    private ParticleSystem meleeParticleSystem;

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
    }

    public void CycleArrow()
    {
        // Cycle through arrows
        selectedArrow++;
        if (selectedArrow >= arrows.Length)
            selectedArrow = 0;
        
        // Update hud
        HUDController.Instance.SetArrow(arrows[selectedArrow].GetComponent<Arrow>().ArrowName);
    }

    // "Charge" arrow based on how long the player is holding the trigger
    public void ChargeArrow()
    {
        // The longer the player holds the trigger the slower strength builds up
        chargeTime += Time.deltaTime * chargeMultiplier;
        strength = Mathf.Pow(chargeTime, 1/chargeMultiplier);
        
        // Change fov to match arrow charge
        thirdPersonCamera.m_Lens.FieldOfView = strength * 4 + 45;
    }

    public void FireArrow()
    {
        Vector3 spawnPosition = transform.position;
        Vector3 arrowDirection = CalculateAimPosition(spawnPosition);

        var arrowInstance = Instantiate(arrows[selectedArrow], spawnPosition, Quaternion.LookRotation(arrowDirection));
            
        // Add force to the arrow equal to strength
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        
        // Reset FOV and vars
        thirdPersonCamera.m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
    }
    
    public void Melee()
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
