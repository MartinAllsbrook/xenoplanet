using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private ItemCounter[] initialItemsArray;
    [SerializeField] private CraftingRecipe[] initialRecipesArray;
    // [SerializeField] private float moveCooldown;
    
    private Dictionary<string, ItemCounter> _itemCounters;
    private Dictionary<string, CraftingRecipe> _craftingRecipes;
    private ItemCounter _selectedItemCounter;
    private int _selectedCounterIndex = 2;
    private bool _readyToMove;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _itemCounters = new Dictionary<string, ItemCounter>();
        _craftingRecipes = new Dictionary<string, CraftingRecipe>();

        foreach (var itemCounter in initialItemsArray)
            _itemCounters.Add(itemCounter.gameObject.name, itemCounter);

        foreach (var craftingRecipe in initialRecipesArray)
        {
            craftingRecipe.MakeDictionary();
            _craftingRecipes.Add(craftingRecipe.GetName(), craftingRecipe);
        }
    }

    private void Start()
    {
        _selectedItemCounter = initialItemsArray[_selectedCounterIndex];
        _selectedItemCounter.ToggleSelected();
    }

    public bool AddItem(string itemName)
    {
        return _itemCounters[itemName].AddItem();
    }

    private bool CraftItem(string itemToCraft)
    {
        var recipe = _craftingRecipes[itemToCraft].GetRequiredItems();
        var requiredItems = recipe.Keys;
        
        // Check to make sure player has the required items 
        foreach (var requiredItem in requiredItems)
        {
            int itemCount = recipe[requiredItem];
            Debug.Log(requiredItem + " : " + itemCount);
            
            if (!_itemCounters[requiredItem].CheckAmount(itemCount))
                return false;
        }

        foreach (var requiredItem in requiredItems)
        {
            int itemCount = recipe[requiredItem];
            _itemCounters[requiredItem].RemoveItems(itemCount);
        }

        if (!_itemCounters[itemToCraft].AddItem())
        {
            // TODO: Make this drop the crafted item.
            Debug.LogError(itemToCraft + " item slot full!");
        }
        
        return true;
    }

    private void OnCraft()
    {
        if (!CraftItem("Arrows"))
        {
            Debug.Log("Not Enough Items");
            // TODO: Make this display a message on the hud instead
        }
    }
    
    public void GetInventoryInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            Vector2 moveDirection = context.ReadValue<Vector2>();
            
            if (moveDirection.x > 0 && _readyToMove)
            {
                MoveSelected(false);
                _readyToMove = false;
            }
            else if (moveDirection.x < 0 && _readyToMove)
            {
                MoveSelected(true);
                _readyToMove = false;
            }

            if (moveDirection.x == 0f && !_readyToMove)
            {
                _readyToMove = true;
            }
        }
    }

    private void MoveSelected(bool moveLeft)
    {
        // Calculate index
        if (moveLeft)
        {
            if (_selectedCounterIndex <= 0)
                _selectedCounterIndex = initialItemsArray.Length - 1;
            else
                _selectedCounterIndex--;
        }
        else
        {
            if (_selectedCounterIndex >= initialItemsArray.Length - 1)
                _selectedCounterIndex = 0;
            else
                _selectedCounterIndex++;
        }
        
        _selectedItemCounter.ToggleSelected();
        _selectedItemCounter = initialItemsArray[_selectedCounterIndex];
        _selectedItemCounter.ToggleSelected();
    }
    
    /*public void GetOpenInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            
        }
    }*/
    
    /*public bool PickUpItem(string itemName)
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
