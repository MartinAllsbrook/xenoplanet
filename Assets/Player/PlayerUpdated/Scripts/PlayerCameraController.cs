using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimRotationSpeed;

    [SerializeField] private float baseCameraHeight;
    [SerializeField] private float crouchCameraHeight;

    private Quaternion cameraRotation;

    private bool _crouching;

    private void Start()
    {
        cameraRotation = cameraFollow.rotation;
    }

    public void SetCameraRotation(Vector2 input, bool aimInput)
    {
        if (aimInput)
        {
            cameraRotation *= Quaternion.AngleAxis(input.x * aimRotationSpeed, Vector3.up);
            cameraRotation *= Quaternion.AngleAxis(input.y * -aimRotationSpeed, Vector3.right);
        }
        else
        {
            cameraRotation *= Quaternion.AngleAxis(input.x * rotationSpeed, Vector3.up);
            cameraRotation *= Quaternion.AngleAxis(input.y * -rotationSpeed, Vector3.right);
        }


        var angles = cameraRotation.eulerAngles;
        angles.z = 0;

        var verticalAngle = cameraRotation.eulerAngles.x;
        if (verticalAngle > 180 && verticalAngle < 340)
        {
            angles.x = 340;
        }
        else if (verticalAngle < 180 && verticalAngle > 40)
        {
            angles.x = 40;
        }

        cameraRotation.eulerAngles = angles;
        cameraFollow.rotation = cameraRotation;
    }

    public void SetCrouch(bool crouch)
    {
        float cameraHeight;
        
        if (crouch && !_crouching)
            cameraHeight = crouchCameraHeight;
        else if (!crouch && _crouching)
            cameraHeight = baseCameraHeight;
        else 
            return;

        _crouching = crouch;
        cameraFollow.position = transform.position + Vector3.up * cameraHeight;
    }
}
