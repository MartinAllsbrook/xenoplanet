using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int maxCount;
    [SerializeField] private text
    private int _itemCount;

    public bool CompareName(string testName)
    {
        return testName == itemName;
    }

    public bool AddItem()
    {
        if (_itemCount < maxCount)
        {
            _itemCount++;
            return true;
        }

        return false;
    }

    public bool GetItem()
    {
        if (_itemCount > 0)
        {
            _itemCount--;
            return true;
        }

        return false;
    }


}
