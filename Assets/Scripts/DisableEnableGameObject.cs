using UnityEngine;

public class DisableEnableGameObject : MonoBehaviour
{
    [Tooltip("The GameObject to disable and enable.")]
    [SerializeField] private GameObject targetGameObject;

    private void OnDisable()
    {
        targetGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke("EnableTargetGameObject", 0.5f);
    }

    private void EnableTargetGameObject()
    {
        targetGameObject.SetActive(true);
    }
}
