using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftingRecipe
{
    [SerializeField] private string[] requiredItems;
    [SerializeField] private string result;

    public string[] GetRequiredItems()
    {
        return requiredItems;
    }

    public bool CompareName(string testName)
    {
        return testName == result;
    }
}
