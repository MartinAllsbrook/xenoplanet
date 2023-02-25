using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int maxCount;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI countDisplay;
    
    private int _itemCount;

    private void Start()
    {
        nameDisplay.text = itemName;
        countDisplay.text = _itemCount.ToString();
    }

    public bool CompareName(string testName)
    {
        return testName == itemName;
    }

    public bool CompareAndAdd(string testName)
    {
        if (testName != itemName)
            return false;

        return AddItem();
    }

    public bool AddItem()
    {
        if (_itemCount < maxCount)
        {
            _itemCount++;
            countDisplay.text = _itemCount.ToString();
            return true;
        }

        return false;
    }

    public bool GetItem()
    {
        if (_itemCount > 0)
        {
            _itemCount--;
            countDisplay.text = _itemCount.ToString();
            return true;
        }

        return false;
    }


}
