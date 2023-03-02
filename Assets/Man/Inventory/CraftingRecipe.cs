using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftingRecipe
{
    [SerializeField] private string[] requiredItems;
    [SerializeField] private int[] requiredItemCounts;
    [SerializeField] private string result;

    private Dictionary<string, int> _recipeDictionary;

    public void MakeDictionary()
    {
        _recipeDictionary = new Dictionary<string, int>();

        for (int i = 0; i < requiredItems.Length; i++)
        {
            _recipeDictionary.Add(requiredItems[i], requiredItemCounts[i]);
        }
    }
    
    public Dictionary<string, int> GetRequiredItems()
    {
        return _recipeDictionary;
    }

    public string GetName()
    {
        return result;
    }
}
