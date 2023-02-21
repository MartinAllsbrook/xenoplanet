using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDrop : MonoBehaviour
{
    [SerializeField] private float playerOffset;
    private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_playerTransform.position.x, playerOffset, _playerTransform.position.z);
    }
}
