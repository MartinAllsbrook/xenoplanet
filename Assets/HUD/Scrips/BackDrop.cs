using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDrop : MonoBehaviour
{
    [SerializeField] private float playerOffset;
    [SerializeField] private Transform _playerTransform;
    void Update()
    {
        transform.position = new Vector3(_playerTransform.position.x, playerOffset, _playerTransform.position.z);
    }
}
