using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private ItemCounter[] initialItemsArray;

    private Dictionary<string, ItemCounter> _itemCounters;

    // [SerializeField] private ItemCounter[] itemCounters;
    // [SerializeField] private CraftingRecipe[] craftingRecipes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _itemCounters = new Dictionary<string, ItemCounter>();

        foreach (var itemCounter in initialItemsArray)
        {
            Debug.Log("ItemCounter Name: " + itemCounter.name);
            Debug.Log("GameObject Name: " + itemCounter.gameObject.name);
            _itemCounters.Add(itemCounter.gameObject.name, itemCounter);
        }
    }

    public bool AddItem(string itemName)
    {
        return _itemCounters[itemName].AddItem();
    }

    /*public bool PickUpItem(string itemName)
    {
        for (int i = 0; i < itemCounters.Length; i++)
        {
            if (itemCounters[i].CompareAndAdd(itemName))
                return true;
        }

        return false;
    }

    public void TestCraft(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            CraftItem("Arrow");
        }
    }

    private void CraftItem(string itemName)
    {
        for (int i = 0; i < craftingRecipes.Length; i++)
        {
            if (craftingRecipes[i].CompareName(itemName))
            {
                Debug.Log("Found Recipe");
                CheckAndRemoveItems(craftingRecipes[i].GetRequiredItems());
                PickUpItem(itemName);
            }
        }
    }

    private void CheckAndRemoveItems(string[] requiredItems)
    {
        if (CheckItems(requiredItems))
        {
            foreach (var item in requiredItems)
            {
                foreach (var itemCounter in itemCounters)
                {
                    itemCounter.CompareAndRemove(item);
                }
            }
        }
    }

    /#1#/Assuming you have a array of ItemCounters called itemCounters
    public bool CheckItems(string[] items)
    {
        //Loop through each string in the array
        for (int i = 0; i < items.Length; i++)
        {
            int itemCount = GetNumDuplicates(items, i);
            
            //Loop through each ItemCounter in the list
            foreach (ItemCounter ic in itemCounters)
            {
                //If the ItemCounter has the same name as s, check its amount
                if (ic.CompareName(items[i]))
                {
                    //If its amount is less than count, return false
                    if (!ic.CheckAmount(itemCount))
                        return false;
                }
            }
        }
        //If no false condition was met, return true
        return true;
    }

    //Loop through the array again and compare each element with item, return count
    private int GetNumDuplicates(string[] items, int index)
    {
        string item = items[index];

        int count = 0;
        
        for (int j = index; j < items.Length; j++)
        {
            if (item == items[j])
                count++;
        }

        return count;
    }#1#
    
    //Assuming you have a array of ItemCounters called itemCounters
    public bool CheckItems(string[] items)
    {
        //Use a dictionary to store the counts of each item
        var itemCounts = new Dictionary<string, int>();
    
        //Loop through each string in the array
        for (int i = 0; i < items.Length; i++)
        {
            //Increment the count of the current item or add it to the dictionary if not present
            if (itemCounts.ContainsKey(items[i]))
                itemCounts[items[i]]++;
            else
                itemCounts.Add(items[i], 1);
        }
    
        //Loop through each ItemCounter in the list
        foreach (ItemCounter ic in itemCounters)
        {
            //If the dictionary contains the ItemCounter's name, check its amount
            if (itemCounts.TryGetValue(ic.GetName(), out int itemCount))
            {
                //If its amount is less than count, return false
                if (!ic.CheckAmount(itemCount))
                    return false;
            }
        }
    
        //If no false condition was met, return true
        return true;
    }*/
}
