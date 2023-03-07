using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCounter : MonoBehaviour
{
    // [SerializeField] private string itemName;
    [Header("Settings")]
    [SerializeField] private int maxCount;
    [SerializeField] private Color selectedColor;
    
    [Header("References")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI countDisplay;

    private int _itemCount;
    private Color _unselectedColor;
    private bool _selected;

    private void Start()
    {
        nameDisplay.text = gameObject.name;
        countDisplay.text = _itemCount.ToString();
        _unselectedColor = backgroundImage.color;
    }

    public bool UpdateCount(int delta)
    {
        int newCount = _itemCount + delta;
        if (newCount <= maxCount && newCount >= 0)
        {
            _itemCount = newCount;
            countDisplay.text = _itemCount.ToString();
            return true;
        }

        return false;
    }

    /*public bool RemoveItems(int numItems)
    {
        if (_itemCount - numItems >= 0)
        {
            _itemCount -= numItems;
            countDisplay.text = _itemCount.ToString();
            return true;
        }
        
        Debug.LogError("Somehow player doesn't have enough items");
        return false;
    }*/

    public bool CheckAmount(int amount)
    {
        if (amount <= _itemCount)
            return true;
        
        return false;
    }

    public int GetAmmount()
    {
        return _itemCount;
    }

    public void ToggleSelected()
    {
        if (_selected)
        {
            backgroundImage.color = _unselectedColor;
            _selected = false;
        }
        else
        {
            backgroundImage.color = selectedColor;
            _selected = true;
        }
    }
}