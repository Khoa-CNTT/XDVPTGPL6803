using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <c>Inventory</c> holds and managed a list of slots which can contain an item.
/// </summary>
public class Inventory : MonoBehaviour
{
    // Actions -> Used for Callback in other scripts
    public static Action OnPlayerInventoryChanged;
    public static Action OnPlayerItemAdded;
    public static Action OnPlayerItemRemoved;

    // Serialized
    [Header("Inventory")]
    [Tooltip("The number of slots a player has in the inventory.")]
    [SerializeField]
    int numberOfSlots = 20;

    [Tooltip("Maximal count of items in ONE slot. How big is one stack.")]
    [SerializeField]
    int maxSpaceOfSlots = 10;

    [Tooltip("Is this inventory owned by the player")]
    [SerializeField]
    bool isPlayerInventory = true;

    [Tooltip("List of the slots ('The inventory').")]
    [SerializeField]
    List<Slot> inventory = new List<Slot>();

    public static Inventory PlayerInstance { get; private set; } // instance of PlayerInventory

    public List<Slot> GetInventory() => inventory;

    /// <summary>
    /// Add slots to the inventory. You can extend the inventory space.
    /// </summary>
    /// <param name="_numberOfSlots">The count of slots which should be added to the inventory</param>
    public void AddSlots(int _numberOfSlots)
    {
        for (int i = 0; i < _numberOfSlots; i++)
        {
            inventory.Add(new Slot());
        }
        if(isPlayerInventory)
            OnPlayerInventoryChanged?.Invoke();
    }

    /// <summary>
    /// <c>GetEmptySlot</c> gives you the first empty slot in the inventory.
    /// </summary>
    /// <returns>An empty slot of the inventory</returns>
    Slot GetEmptySlot()
    {
        foreach (Slot slot in inventory)
        {
            if (slot.GetItem() == null)
                return slot;
        }
        return null;
    }

    /// <summary>
    /// <c>GetSlotByItem</c> lets you find a slot by the item it's containing.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Slot with the specified item.</returns>
    Slot GetSlotByItem(Item item)
    {
        foreach (Slot slot in inventory)
        {
            if (slot.GetItem() != null)
            {
                if (slot.GetItem().id == item.id)
                    return slot;
            }

        }
        return null;
    }

    /// <summary>
    /// <c>GetRest</c> calculates the rest count of an item and a slot.
    /// Example: Slot has a capacity of 20. Count of item is 22. Returns 2.
    /// </summary>
    /// <returns>The rest count of an item and a slot.</returns>
    int GetRest(int count, Slot slot)
    {
        if (CountFitsInSlot(count, slot))
        {
            return 0;
        }
        else
        {
            return (count - (maxSpaceOfSlots - slot.GetCount()));
        }
    }

    /// <summary>
    /// <c>CountFitsInSlot</c> calculates if the item count fits in the slot capacity
    /// </summary>
    /// <param name="count"></param>
    /// <param name="slot"></param>
    /// <returns>True: Count fits. False: Count does not fit.</returns>
    bool CountFitsInSlot(int count, Slot slot)
    {
        int leftSize = maxSpaceOfSlots - slot.GetCount();
        if (leftSize > count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// <c>IndexIsInRange</c> looks if an index of the inventory list is in range.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>True: Is in range. False: Is not in range.</returns>
    public bool IndexIsInRange(int index)
    {
        // Returns true if a given index is in the bounds of the inventory.
        // Example (maxSize = 10) index = -10 : false , index =  100: false, index = 7 : true 

        if (index < inventory.Count && index >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// <c>Remove</c> removes an item/s of the inventory list. It can be based in a specific index.
    /// </summary>
    /// <param name="itemType">which item should be removed</param>
    /// <param name="count">Count of the item you want to remove</param>
    /// <param name="invIndex">(optional) Index of the inventory list</param>
    /// <returns>The rest of the count, which could not be removed.</returns>
    public int Remove(Item itemType, int count, int invIndex = -1)
    {
        Slot slot = null;

        Item item = Instantiate(itemType);
        if (item == null)
            return -1;

        // Get Slot
        if (invIndex > -1)
        {
            if (IndexIsInRange(invIndex))
                slot = inventory[invIndex];
        }
        else
        {
            slot = GetSlotByItem(item);
        }

        if (slot == null)
            return -1;

        // remove
        if (slot.GetItem() != null && slot.GetItem().id == item.id) // Wenn im Slot schon das gleiche item ist
        {
            int rest = 0;

            if (slot.GetCount() >= count)
            {
                slot.RemoveCount(count);
            }
            else
            {
                rest = count - slot.GetCount();
                slot.Clear();
            }

            if (isPlayerInventory)
            {
                OnPlayerInventoryChanged?.Invoke();
                OnPlayerItemRemoved?.Invoke();
            }
            
            return rest;
        }
        else
            return -1;
    }

    /// <summary>
    /// <c>Add</c> adds an item/s of the inventory list. It can be based in a specific index.
    /// </summary>
    /// <param name="itemType">which item should be added</param>
    /// <param name="count">Count of the item you want to add</param>
    /// <param name="invIndex">(optional) Index of the inventory list</param>
    /// <returns>The rest of the count, which could not be added.</returns>
    public int Add(Item itemType, int count, int invIndex = -1)
    {
        int rest = 0;
        Slot slot = null;
        
        Item item = Instantiate(itemType);
        if (item == null)
            return -1;

        // Get Slot
        if (invIndex > -1)
        {
            if(IndexIsInRange(invIndex))
                slot = inventory[invIndex];
        }
        else
        {
            slot = GetSlotByItem(item);
            if (slot == null)
                slot = GetEmptySlot();
        }

        if (slot == null)
            return -1;

        // add
        if (slot.GetItem() != null && slot.GetItem().id == item.id) // Wenn im Slot schon das selbe item ist
        {
            if (CountFitsInSlot(count, slot))
            {
                slot.AddCount(count);
            }
            else
            {
                rest = GetRest(count, slot);
                slot.Set(item, maxSpaceOfSlots);
            }
        }
        else if (slot.GetItem() == null)
        {
            if (CountFitsInSlot(count, slot))
            {
                slot.Set(item, count);
            }
            else
            {
                rest = GetRest(count, slot);
                slot.Set(item, maxSpaceOfSlots);
            }
        }
        else
            return -1;

        if (isPlayerInventory)
        {
            OnPlayerInventoryChanged?.Invoke();
            OnPlayerItemAdded?.Invoke();
        }
       
        return rest;
    }


    void Awake()
    {
        // Initialize Inventory Slots
        AddSlots(numberOfSlots);

        if (isPlayerInventory)
            PlayerInstance = this;
    }
}
