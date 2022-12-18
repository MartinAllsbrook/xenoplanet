using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    public void FireArrow(float strength)
    {
        Debug.Log(transform.rotation);
        Vector3 rotation = transform.rotation.eulerAngles + new Vector3(90, 0, 0);
        Instantiate(arrow, transform.position, Quaternion.Euler(rotation));
    }
}
