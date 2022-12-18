using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    private GameObject arrow;
    private PlayerControls _playerControls;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerControls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
//Press Right Trigger (Left Click) - Fire
        if (_playerControls.Player.Fire.IsInProgress())
        {
            
        }

        var released = _playerControls.Player.Fire.WasReleasedThisFrame();
        Debug.Log(_playerControls.Player.Fire.IsInProgress());
        if (released)
        {
            // Fire arrow
            Debug.Log("fire");
            Instantiate(arrow, transform.position, transform.rotation);
        }
    }
}
