using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemCounter[] itemCounters;
    [SerializeField] private CraftingRecipe[] craftingRecipes;
    private void Start()
    {
        int numItems = 5;
        itemCounters = new ItemCounter[numItems];


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
