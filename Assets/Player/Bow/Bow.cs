using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
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
    [SerializeField] private float maxAimDistance;
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeDamage;
    [SerializeField] private float maxChargeTime;
    
    [SerializeField] private GameObject mainCamera;
    // [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private Transform arrowAimer;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    // [SerializeField] private TMP_Text _numArrowsText;
    
    private float _chargeTime = 0;
    private float _strength = 0;
    private int _selectedArrowIndex = 0;
    private ParticleSystem _meleeParticleSystem;
    private bool _chargingInput = false;
    private bool _chargeArrow = false;
    private bool _aimingInput = false;
    private float _numArrows;
    
    public bool isAiming;
    private bool _readyToFire = true;
    private bool _aimingCoroutinesRunning;

    #region Basic Unity Event Functions

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _meleeParticleSystem = GetComponent<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /*private void Update()
    {
        // GetNumArrows();
        if (_chargingInput)
            ChargeArrow();
    }*/

    #endregion

    #region Get Inputs

    // Gets the right trigger input and uses it to control the bow
    public void GetChargingInput(InputAction.CallbackContext context)
    {
        // Will be true if trigger is held, false if not // Shouldn't be necessary with rework 
        /*_chargingInput = context.action.WasPerformedThisFrame(); 
        if (context.action.WasReleasedThisFrame() && _readyToFire)
        {
            _readyToFire = false;
            FireArrow();
            StartCoroutine(ReadyBow());
        }*/

        if (context.action.WasPressedThisFrame()) // On RT down
        {
            _chargeArrow = true;
            StartCoroutine(FireArrowRoutine());
        }

        if (context.action.WasReleasedThisFrame())
        {
            _chargeArrow = false;
        }
    }

    /*IEnumerator ReadyBow()
    {
        yield return new WaitForSeconds(0.02f);
        _readyToFire = true;
    }*/

    public void GetMeleeInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
            Melee();
    }
    
    public void GetCycleInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
            CycleArrow();
    }

    #endregion

    IEnumerator FireArrowRoutine()
    {
        float deltaTime = 0.01f;
        float chargeTime = 0f;
        float strength = 0f;
        
        while (_chargeArrow) // Eventually charge arrow will be set to false by GetChargeInput()
        {
            chargeTime += deltaTime;
            strength = 1 - maxChargeTime / (chargeTime + maxChargeTime);

            yield return new WaitForSeconds(deltaTime);
        }
        
        // Fire the arrow now that it is done charging this should be a separate method
        Vector3 spawnPosition = Player.Instance.transform.position + Vector3.up * 2;
        Vector3 arrowDirection = CalculateAimPosition(spawnPosition);

        var arrowInstance = Instantiate(arrows[_selectedArrowIndex], spawnPosition, Quaternion.LookRotation(arrowDirection));
        arrowInstance.GetComponent<Arrow>().Fire(strength); // Add force to the arrow equal to strength using arrow API
        
        Inventory.Instance.RemoveItem(arrows[_selectedArrowIndex].name + 's');  // Remove arrow from inventory
        impulseSource.GenerateImpulse(); // Generate an impulse when arrows are fired

        yield return null;
    }

    private void CycleArrow()
    {
        // Start by cycling arrow index
        _selectedArrowIndex++;
        if (_selectedArrowIndex >= arrows.Length)
            _selectedArrowIndex = 0;
        
        // Update stuff
        GameObject arrowGameObject = arrows[_selectedArrowIndex].gameObject;
        
        _numArrows = Inventory.Instance.GetItemCount(arrowGameObject.name);
        HUDController.Instance.SetArrow(arrowGameObject.name);
    }
    
    /*// "Charge" arrow based on how long the player is holding the trigger
    private void ChargeArrow()
    {
        // The longer the player holds the trigger the slower strength builds up
        _chargeTime += Time.deltaTime;
        _strength = 1 - maxChargeTime / (_chargeTime + maxChargeTime);

        // Change fov to match arrow charge
        // thirdPersonCamera.m_Lens.FieldOfView = strength * 4 + 45;
        // ChargeZoom();
    }*/

    /*private void FireArrow()
    {
        Vector3 spawnPosition = Player.Instance.transform.position + Vector3.up * 2;
        Vector3 arrowDirection = CalculateAimPosition(spawnPosition);

        var arrowInstance = Instantiate(arrows[_selectedArrowIndex], spawnPosition, Quaternion.LookRotation(arrowDirection));
        
        arrowInstance.GetComponent<Arrow>().Fire(_strength); // Add force to the arrow equal to strength 
        
        Inventory.Instance.RemoveItem(arrows[_selectedArrowIndex].name + 's');  // Remove arrow from inventory

        // Reset FOV and vars
        // thirdPersonCamera.m_Lens.FieldOfView = 45;
        _strength = 0;
        _chargeTime = 0;
        
        impulseSource.GenerateImpulse(); // Generate an impulse when arrows are fired
    }*/

    /*void GetNumArrows()
    {
        // _numArrows = arrows.Length;
        // _numArrowsText.text = _numArrows.ToString();
    }*/
    
    private void Melee()
    {
        Debug.Log("Melee");
        _meleeParticleSystem.Play();
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