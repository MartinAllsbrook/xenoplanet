using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpdatedBow : MonoBehaviour
{
    private PlayerUpdatedController _playerUpdatedController; // TODO: HMMMMMMM?
    // private CinemachineFreeLook.Orbit[] _startOrbits;
    
    [Header("Cameras")]
    [SerializeField] private GameObject moveCamera;
    [SerializeField] private GameObject aimCamera;

    [Header("References")]
    [SerializeField] private CrosshaireController crosshairController;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Values")]
    [SerializeField] private float chargeTimeCoefficient;
    [SerializeField] private float chargeTimeExponent;
    [SerializeField] private float maxImpulseForce;
    /*[SerializeField] private float fovReduction;
    [SerializeField] private float fov;*/
    
    private float _aimProgress;
    private float _chargeTime = 0f;
    private float _strength = 0f;
    private int _selectedArrowIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _playerUpdatedController = GetComponent<PlayerUpdatedController>();
        // _startOrbits = _playerUpdatedController.thirdPersonCamera.m_Orbits;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Aim(bool input)
    {
        if (input)
        {
            if (!aimCamera.activeInHierarchy)
            {
                aimCamera.SetActive(true);
                moveCamera.SetActive(false);
            }
            ChargeArrow();
        }
        else if(!input && !moveCamera.activeInHierarchy)
        {
            moveCamera.SetActive(true);
            aimCamera.SetActive(false);
            
            _chargeTime = 0;
        }        
    }

    public void Fire()
    {
        Debug.Log("Fire Arrow");
        // Vector3 spawnPosition = Player.Instance.transform.position + Vector3.up * 1.6f; // Could move reference to player instance outside this
        Vector3 spawnPosition = transform.position + Vector3.up * 1.6f; // Could move reference to player instance outside this
        Vector3 arrowDirection = _playerUpdatedController.mainCamera.transform.forward;

        var arrowInstance = Instantiate(arrows[0], spawnPosition, Quaternion.LookRotation(arrowDirection));
        arrowInstance.GetComponent<Arrow>().Fire(_strength); // Add force to the arrow equal to strength using arrow API
        _chargeTime = 0;

        // Inventory.Instance.UpdateItemCount(arrows[_selectedArrowIndex].name + 's', -1);  // Remove arrow from inventory

        impulseSource.GenerateImpulse(_strength * maxImpulseForce); // Generate an impulse when arrows are fired
    }
    
    private void ChargeArrow()
    {
        _chargeTime += Time.fixedDeltaTime; 
        _strength = 1 - 1 / (Mathf.Pow((_chargeTime * chargeTimeCoefficient), chargeTimeExponent) + 1);

        // _playerUpdatedController.aimCamera.m_Lens.FieldOfView = fov - _strength * fovReduction;
        crosshairController.SetCrossHairWidth(_strength);
    }
    
    /*
    private CinemachineFreeLook.Orbit[] LerpOrbitArray(CinemachineFreeLook.Orbit[] a, CinemachineFreeLook.Orbit[] b, float t)
    {
        CinemachineFreeLook.Orbit[] result = new CinemachineFreeLook.Orbit[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = LerpOrbit(a[i], b[i], t);
        }
        return result;
    }
    */
    
    private CinemachineFreeLook.Orbit LerpOrbit(CinemachineFreeLook.Orbit a,CinemachineFreeLook.Orbit b, float t)
    {
        // Debug.Log("lerp");

        CinemachineFreeLook.Orbit result = new CinemachineFreeLook.Orbit();
        result.m_Height = Mathf.Lerp(a.m_Height, b.m_Height, t);
        result.m_Radius = Mathf.Lerp(a.m_Radius, b.m_Radius, t);

        return result;
    }
    
    
}