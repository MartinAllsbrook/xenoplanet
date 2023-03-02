using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    // [SerializeField] private string itemName;
    [SerializeField] private int maxCount;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI countDisplay;
    
    private int _itemCount;

    private void Start()
    {
        nameDisplay.text = gameObject.name;
        countDisplay.text = _itemCount.ToString();
    }

    /*
    public string GetName()
    {
        return itemName;
    }
    */

    /*
    public bool CompareAndAdd(string testName)
    {
        if (testName != itemName)
            return false;

        return AddItem();
    }
    */

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

    /*
    public void CompareAndRemove(string testName)
    {
        if (testName != itemName)
            return;

        _itemCount--;
    }
    */
    
    public void RemoveItems(int numItems)
    {
        if (_itemCount - numItems >= 0)
        {
            _itemCount -= numItems;
            countDisplay.text = _itemCount.ToString();
            return;
        }
        
        Debug.LogError("Somehow player doesn't have enough items");
    }

    public bool CheckAmount(int amount)
    {
        if (amount <= _itemCount)
            return true;
        
        return false;
    }
}
