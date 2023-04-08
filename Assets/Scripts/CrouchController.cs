using UnityEngine;

public class CrouchController : MonoBehaviour
{
    [Tooltip("The transform that the player should look at when crouching.")]
    [SerializeField] private Transform playerLookAt;

    [Tooltip("The collider that should be resized when crouching.")]
    [SerializeField] private CapsuleCollider playerCollider;

    [Tooltip("The height of the collider when the player is not crouching.")]
    [SerializeField] private float standingColliderHeight = 1.8f;
    [Tooltip("The y offset of the collider when the player is not crouching.")]
    [SerializeField] private Vector3 standingCenter;
    [Tooltip("The height of the collider when the player is crouching.")]
    [SerializeField] private float crouchingColliderHeight = 1f;
    [Tooltip("The y offset of the collider when the player is crouching.")]
    [SerializeField] private Vector3 crouchingCenter;

    [Tooltip("The position of the playerLookAt transform when the player is not crouching.")]
    [SerializeField] private Vector3 standingLookAtPosition = new Vector3(0f, 1.6f, 0f);

    [Tooltip("The position of the playerLookAt transform when the player is crouching.")]
    [SerializeField] private Vector3 crouchingLookAtPosition = new Vector3(0f, 0.5f, 0f);

    private bool isCrouching = false;

    // Public method to toggle crouch on and off
    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            playerCollider.height = crouchingColliderHeight;
            playerCollider.center = crouchingCenter;
            playerLookAt.localPosition = crouchingLookAtPosition;
        }
        else
        {
            playerCollider.height = standingColliderHeight;
            playerCollider.center = standingCenter;
            playerLookAt.localPosition = standingLookAtPosition;
        }
    }
}
