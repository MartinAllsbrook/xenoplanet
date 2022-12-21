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
    [SerializeField] private Rigidbody playerRigidBody;

    private float chargeTime = 0;
    private float strength = 0;
    private int selectedArrow = 0;
    private float chargeMultiplier = 3;

    public UnityEvent unHook;
    
    private bool hooked = false;
    public bool Hooked
    {
        get { return hooked; }
        set { hooked = value; }
    }

    private Vector3 hookPosition;
    public Vector3 HookPosition
    {
        get { return hookPosition; }
        set { hookPosition = value; }
    }

    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
        
        // Create unHook event
        if (unHook == null)
            unHook = new UnityEvent();
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
    public void FireArrow()
    {
        // Use camera rotation to aim the arrow
        Vector3 rotation = camera.transform.rotation.eulerAngles;
        var arrowInstance = Instantiate(arrows[selectedArrow], transform.position + new Vector3(0, 2, 0), Quaternion.Euler(rotation));
        
        // Add force to the arrow equal to strength
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        
        // Reset FOV and vars
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
        
        // If shooting a grapple arrow unhook
        if (arrows[selectedArrow].name == "grapple")
            UnHook();
    }

    // Pull on graple
    public void Pull()
    {
        // If the player is hooked pull on the grapple
        if (hooked)
            playerRigidBody.AddForce((hookPosition - transform.position).normalized * 1000);
    }

    public void UnHook()
    {
        // Invoke unhook unity event for arrow
        unHook.Invoke();
        // Reset var
        hooked = false;
    }
}
