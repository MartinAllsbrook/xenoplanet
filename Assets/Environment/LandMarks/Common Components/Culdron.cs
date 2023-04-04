using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culdron : MonoBehaviour
{
    private bool _used = false;
    public void Use()
    {
        if (!_used)
        {
            Player.Instance.AddShield(25);
            _used = true;
        }
    }
}
