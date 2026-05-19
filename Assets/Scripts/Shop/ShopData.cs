using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop/Shop Data")]
public class ShopData : ScriptableObject
{
    [SerializeField] private List<Item> baseInventory = new List<Item>();

    // Runtime copy so never dirty the SO asset itself
    private List<Item> _runTimeInventory = new List<Item>();

    //Utilize this to see the the '_runTimeInventory'
    public IReadOnlyList<Item> RunTimeInventory => _runTimeInventory.AsReadOnly();

    public event Action<IReadOnlyList<Item>> OnInventoryChanged;
    
    public void InitializeRuntime()
    {
        _runTimeInventory = new List<Item>(baseInventory);
        OnInventoryChanged?.Invoke(_runTimeInventory.AsReadOnly());
    } 

    public void AddItem(Item item)
    {
        _runTimeInventory.Add(item);

        //Send event with the _runTimeInventory but keep as ReadOnlyCollection
        OnInventoryChanged?.Invoke(_runTimeInventory.AsReadOnly()); 
    }

    public void RemoveItem(Item item)
    {
        _runTimeInventory.Remove(item);
        OnInventoryChanged?.Invoke(_runTimeInventory.AsReadOnly());
    }

}
