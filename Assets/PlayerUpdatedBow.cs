using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerUpdatedBow : MonoBehaviour
{
    private PlayerUpdatedController _playerUpdatedController;

    private CinemachineFreeLook.Orbit[] _startOrbits;
    private float _aimProgress;

    [SerializeField] private GameObject moveCamera;
    [SerializeField] private GameObject aimCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerUpdatedController = GetComponent<PlayerUpdatedController>();
        _startOrbits = _playerUpdatedController.thirdPersonCamera.m_Orbits;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Aim(bool input)
    {
        if (input && !aimCamera.activeInHierarchy)
        {
            aimCamera.SetActive(true);
            moveCamera.SetActive(false);
        }
        else if(!input && !moveCamera.activeInHierarchy)
        {
            moveCamera.SetActive(true);
            aimCamera.SetActive(false);
        }        
        // _playerUpdatedController.thirdPersonCamera.m_Orbits = LerpOrbitArray(_startOrbits, _aimOrbit, _aimProgress);
    }
    
    private CinemachineFreeLook.Orbit[] LerpOrbitArray(CinemachineFreeLook.Orbit[] a, CinemachineFreeLook.Orbit[] b, float t)
    {
        CinemachineFreeLook.Orbit[] result = new CinemachineFreeLook.Orbit[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = LerpOrbit(a[i], b[i], t);
        }
        return result;
    }
    
    private CinemachineFreeLook.Orbit LerpOrbit(CinemachineFreeLook.Orbit a,CinemachineFreeLook.Orbit b, float t)
    {
        // Debug.Log("lerp");

        CinemachineFreeLook.Orbit result = new CinemachineFreeLook.Orbit();
        result.m_Height = Mathf.Lerp(a.m_Height, b.m_Height, t);
        result.m_Radius = Mathf.Lerp(a.m_Radius, b.m_Radius, t);

        return result;
    }
    
    
}