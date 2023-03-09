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
    [SerializeField] public ItemPickup associatedItemPickup;
    [SerializeField] private RectTransform progressEmpty;
    [SerializeField] private RectTransform progressFull;

    private float _rightPercent;
    private float _leftPercent;
    
    private int _itemCount;
    private Color _unselectedColor;
    private bool _selected;

    private void Start() {
        nameDisplay.text = gameObject.name;
        countDisplay.text = _itemCount.ToString();
        _unselectedColor = backgroundImage.color;

        _rightPercent = progressEmpty.anchorMax.x;
        _leftPercent = progressEmpty.anchorMin.x;
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

    public void UseItem(bool use)
    {
        if (!CheckAmount(1))
            return;
        
        UseHandler useHandler = GetComponent<UseHandler>();

        if (useHandler == null)
        {
            Debug.LogError("No UseHandler attached");
            return;
        }

        if (use)
            useHandler.StartUsing();
        else
            useHandler.StopUsing();
    }

    public void SetUsePercent(float percentComplete)
    {
        float widthPercent = _rightPercent - _leftPercent;

        float fullWidth = widthPercent * percentComplete;
        float emptyWidth = widthPercent * (1 - percentComplete);

        progressEmpty.anchorMin = new Vector2(_rightPercent - emptyWidth, progressEmpty.anchorMin.y);
        progressFull.anchorMax = new Vector2(_leftPercent + fullWidth, progressFull.anchorMax.y);
    }
}
