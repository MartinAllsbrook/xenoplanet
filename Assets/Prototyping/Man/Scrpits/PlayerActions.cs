using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject thirdPersonCamera;
    private float chargeTime = 0;
    private float strength = 0;

    private float chargeMultiplier = 3;
    public void ChargeArrow()
    {
        chargeTime += Time.deltaTime * chargeMultiplier;
        strength = Mathf.Pow(chargeTime, 1/chargeMultiplier);
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = strength * 4 + 45;
    }
    public void FireArrow()
    {
        Vector3 rotation = camera.transform.rotation.eulerAngles;
        var arrowInstance = Instantiate(arrow, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(rotation));
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
    }
}
