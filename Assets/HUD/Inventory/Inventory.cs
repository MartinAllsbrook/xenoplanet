using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public static UnityEvent OnUpdateCount;

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

        if (OnUpdateCount == null)
            OnUpdateCount = new UnityEvent();

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

    public bool UpdateItemCount(string itemName, int deltaCount)
    {
        if (_itemCounters[itemName].UpdateCount(deltaCount))
        {
            OnUpdateCount.Invoke();
            return true;
        }

        return false;
    }

    public int GetItemCount(string itemName)
    {
        return _itemCounters[itemName].GetAmmount();
    }
    
    private bool CraftItem(string itemToCraft)
    {
        var recipe = _craftingRecipes[itemToCraft].GetRequiredItems();
        var requiredItems = recipe.Keys;
        
        // Check to make sure player has the required items 
        foreach (var requiredItem in requiredItems)
        {
            int itemCount = recipe[requiredItem];
            // Debug.Log(requiredItem + " : " + itemCount);
            
            if (!_itemCounters[requiredItem].CheckAmount(itemCount))
                return false;
        }

        foreach (var requiredItem in requiredItems)
        {
            int itemCount = recipe[requiredItem];
            UpdateItemCount(requiredItem, -itemCount); // Negative to remove items
        }

        if (!UpdateItemCount(itemToCraft, 1))
        {
            // TODO: Make this drop the crafted item.
            Debug.LogError(itemToCraft + " item slot full!");
        }
        
        return true;
    }

    private void OnCraft()
    {
        string itemToCraft = _selectedItemCounter.name;
        if (!CraftItem(itemToCraft))
        {
            Debug.Log("Not Enough Items");
            // TODO: Make this display a message on the hud instead
        }
    }

    private void OnDrop()
    {
        var itemCounter = _selectedItemCounter;
        
        // Remove item from inventory, return if item cannot be removed
        if (!itemCounter.UpdateCount(-1))
            return;

        var playerTransform = Player.Instance.transform;
        Vector3 spawnPosition = playerTransform.position + (playerTransform.forward * 2) + (Vector3.up * 5);
        Instantiate(itemCounter.ItemPickup, spawnPosition, new Quaternion(0,0,0,0));
    }

    private void OnUse(bool use)
    {
        if (use)
            _selectedItemCounter.UseItem(use);
        else
            _selectedItemCounter.UseItem(use);
    }

    #region Get Inputs

    public void GetCraftInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
            OnCraft();    
    }

    public void GetDropInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
            OnDrop();
    }

    public void GetUseInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
            OnUse(true);

        if (context.action.WasReleasedThisFrame())
            OnUse(false);
    }

    #endregion
    
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
    
}
