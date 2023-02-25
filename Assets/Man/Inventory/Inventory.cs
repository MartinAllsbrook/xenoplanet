using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    
    [SerializeField] private ItemCounter[] itemCounters;
    [SerializeField] private CraftingRecipe[] craftingRecipes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool PickUpItem(string itemName)
    {
        for (int i = 0; i < itemCounters.Length; i++)
        {
            if (itemCounters[i].CompareAndAdd(itemName))
                return true;
        }

        return false;
    }

    private void CraftItem(string itemName)
    {
        for (int i = 0; i < craftingRecipes.Length; i++)
        {
            if (craftingRecipes[i].CompareName(itemName))
            {
                // CraftingRecipe craftingRecipe = craftingRecipes[i];
                // string[] requiredItems = craftingRecipe.GetRequiredItems();
                CheckItems(craftingRecipes[i].GetRequiredItems());
            }
        }
    }

    private void CheckItems(string[] requiredItems)
    {
        List<string> retrievedItems = new List<string>();
        foreach (var item in requiredItems)
        {
            foreach (var counter in itemCounters)
            {
                if (counter.CompareName(item))
                {
                    if (counter.GetItem())
                    {
                        retrievedItems.Add(item);
                    }
                }
            }
        }
        
    }
}
