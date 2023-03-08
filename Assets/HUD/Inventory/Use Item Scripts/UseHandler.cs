using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHandler : MonoBehaviour
{
    [SerializeField] private float useTime;
    
    private bool _using;
    private float _timePassed;
    
    public void StartUsing()
    {
        Debug.Log("Start using, Using: " + _using);
        
        if (!_using)
            _using = true;
    }

    public void StopUsing()
    {
        Debug.Log("Stop using Using: " + _using);

        _using = false;
        _timePassed = 0;
    }

    private void Update()
    {
        if (_timePassed >= useTime)
        {
            StopUsing();
            UseItem();
            return;
        }

        if (_using)
        {
            _timePassed += Time.deltaTime;
            Debug.Log("Time Passed: " + _timePassed);
        }
    }

    protected virtual void UseItem()
    {
        GetComponent<ItemCounter>().UpdateCount(-1);
    }
}
