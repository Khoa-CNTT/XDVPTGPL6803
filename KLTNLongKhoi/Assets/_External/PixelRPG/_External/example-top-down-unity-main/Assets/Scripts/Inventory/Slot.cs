using System;
using UnityEngine;

/// <summary>
/// A Slot can contain an item and its count. Used for the inventory.
/// </summary>
[Serializable]
public class Slot
{
    [Header("Slot")]
    [Tooltip("The item which is in this slot")]
    [SerializeField]
    Item item = null;
    
    [Tooltip("The count of the contained item")]
    [SerializeField]
    int count = 0;

    #region GETTER & SETTER
    public Item GetItem() => item;
    public void SetItem(Item _item) => item = _item;
    public int GetCount() => count;
    public void SetCount(int newCount)
    {
        count = newCount;
        if (count <= 0)
            Clear();
    }
    #endregion
    
    
    /// <summary>
    /// <c>AddCount</c> adds a value to the current count. The slot does not have any logic for the maximum stack size.
    /// </summary>
    /// <param name="value">Value which should be added.</param>
    public void AddCount(int value) => count += value;

    /// <summary>
    /// <c>RemoveCount</c> removes a value from the current count. It clears the slot if count == 0.
    /// </summary>
    /// <param name="value">Value which should be removed.</param>
    public void RemoveCount(int value)
    {
        count -= value; 
        if (count <= 0)
            Clear();
    }
    
    /// <summary>
    /// Sets the count to 0
    /// </summary>
    public void ResetCount() => count = 0;

    /// <summary>
    /// Clears the hole slot
    /// </summary>
    public void Clear()
    {
        item = null;
        count = 0;
    }
    
    /// <summary>
    /// Gives you a copy of this slot.
    /// </summary>
    /// <returns>Copy of this slot</returns>
    public Slot Copy()
    {
        Slot slot = new Slot();
        slot.SetCount(count);
        slot.SetItem(item);

        return slot;
    }
    
    /// <summary>
    /// Sets the item and count of this slot.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_count"></param>
    public void Set(Item _item, int _count = 1)
    {
        item = _item;
        count = _count;
    }
}
