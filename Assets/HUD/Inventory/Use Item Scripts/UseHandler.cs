using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHandler : MonoBehaviour
{
    [SerializeField] private float useTime;
    
    private bool _using;
    private float _timePassed;
    private ItemCounter _itemCounter;

    private void Start()
    {
        _itemCounter = GetComponent<ItemCounter>();
    }

    public void StartUsing()
    {
        // Debug.Log("Start using, Using: " + _using);
        if (!_using)
            _using = true;
    }

    public void StopUsing()
    {
        // Debug.Log("Stop using Using: " + _using);
        _itemCounter.SetUsePercent(0);
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
            _itemCounter.SetUsePercent(_timePassed/useTime);
            // Debug.Log("Time Passed: " + _timePassed);
        }
    }

    protected virtual void UseItem()
    {
        _itemCounter.UpdateCount(-1);
    }
}
