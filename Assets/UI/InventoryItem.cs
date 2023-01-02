using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField] public Sprite Icon;
    public delegate void GenericDelegate();

    public GenericDelegate Callback;

    public void UseItem()
    {
        Callback();
        Debug.Log("Item Used");
    }
}
