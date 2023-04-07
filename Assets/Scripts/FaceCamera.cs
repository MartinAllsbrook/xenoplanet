using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Tooltip("The speed at which the object rotates to face the camera.")]
    public float rotationSpeed = 5f;

    private Transform cameraTransform;

    private void Start()
    {
        // Find the camera transform by tag
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        // Calculate the direction to the camera
        Vector3 direction = cameraTransform.position - transform.position;

        // Calculate the rotation to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target rotation
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
