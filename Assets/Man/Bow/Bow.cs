using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bow : MonoBehaviour
{
    public static Bow Instance;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private Transform arrowSpawnPosition;
    [SerializeField] private LayerMask aimingLayerMask;
    [SerializeField] private Transform arrowAimer;
    [SerializeField] private float maxAimDistance;
    private float chargeTime = 0;
    private float strength = 0;
    private int selectedArrow = 0;
    private float chargeMultiplier = 3;

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {

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
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = strength * 4 + 45;
    }

    public Vector3 CalculateAimPosition(Vector3 spawnPosition)
    {
        // Create a raycast from camera to see where it hit's an invisible sphere around the player 
        Vector3 origin = camera.transform.position;
        Vector3 direction = camera.transform.forward;
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimingLayerMask))
            arrowAimer.position = hit.point;
        else
            arrowAimer.position = camera.transform.position + camera.transform.forward * maxAimDistance;

        return arrowAimer.position - spawnPosition;
    }
    
    public void FireArrow()
    {
        Vector3 spawnPosition = arrowSpawnPosition.position;
        Vector3 arrowDirection = CalculateAimPosition(spawnPosition);

        var arrowInstance = Instantiate(arrows[selectedArrow], spawnPosition, Quaternion.LookRotation(arrowDirection));
            
        // Add force to the arrow equal to strength
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        
        // Reset FOV and vars
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
    }
}
