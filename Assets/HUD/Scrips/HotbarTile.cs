using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HotbarTile : MonoBehaviour
{
    [SerializeField] public ItemSlot itemSlot;
    [SerializeField] private Color selectedColor;
    
    private Color _unselectedColor;

    private Image _backGroundImage;

    private bool _selected;
    public bool Selected
    {
        get { return _selected; }
        set
        {
            if (_selected && !value)
            {
                _selected = false;
                Deselect();
            }
            else if (!_selected && value)
            {
                _selected = true;
                Select();
            }
        }
    }
    private void Awake()
    {
        _backGroundImage = gameObject.GetComponent<Image>();
        _unselectedColor = _backGroundImage.color;
    }

    private void Select()
    {
        Debug.Log("Select");
        _backGroundImage.color = selectedColor;
    }

    private void Deselect()
    {
        _backGroundImage.color = _unselectedColor;
    }
}
