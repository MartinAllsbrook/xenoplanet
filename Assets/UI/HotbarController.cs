using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private HotbarTile[] _hotbarTiles;

    private int _selectedTile = 0;
    // public int SelectedTile
    // {
    //     private get { return _selectedTile; }
    //     set
    //     {
    //         if (value > _selectedTile)
    //             NextTile();
    //         else if (value < _selectedTile)
    //             PrevTile();
    //     }
    // }

    private void Start()
    {
        InputManager.Instance.hotbarNext.AddListener(NextTile);
        InputManager.Instance.hotbarPrev.AddListener(PrevTile);
        _hotbarTiles[0].Selected = true;
    }

    private void NextTile()
    {
        Debug.Log("Next");
        if (_selectedTile >= _hotbarTiles.Length - 1)
            _selectedTile = 0;
        else
            _selectedTile++;
        
        SetTile(_selectedTile);
    }

    private void PrevTile()
    {
        if (_selectedTile <= 0)
            _selectedTile = _hotbarTiles.Length - 1;
        else
            _selectedTile--;
        
        SetTile(_selectedTile);
    }

    private void SetTile(int tileIndex)
    {
        for (int i = 0; i < _hotbarTiles.Length; i++)
        {
            if (i == _selectedTile)
                _hotbarTiles[i].Selected = true;
            else
                _hotbarTiles[i].Selected = false;
        }
    }
}
