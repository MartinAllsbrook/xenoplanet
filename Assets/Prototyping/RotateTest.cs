using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{

    [SerializeField] private float rotateAmount;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateAmount * Time.deltaTime, 0);
    }
};
