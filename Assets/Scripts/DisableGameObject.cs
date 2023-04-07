using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    [Tooltip("The gameobject to disable/enable.")]
    [SerializeField] private GameObject objectToDisable;

    private void OnDisable()
    {
        objectToDisable.SetActive(false);
    }

    private void OnEnable()
    {
        objectToDisable.SetActive(true);
    }
}
