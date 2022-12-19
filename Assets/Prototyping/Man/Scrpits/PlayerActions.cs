using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject camera;
    
    public void FireArrow(float strength)
    {
        Debug.Log(transform.rotation);
        Vector3 rotation = camera.transform.rotation.eulerAngles + new Vector3(90, 0, 0);
        Instantiate(arrow, transform.position + new Vector3(0, 2, 0) + Vector3.forward, Quaternion.Euler(rotation));
    }
}
