using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool TryAddItem(GameObject gameObject)
    {
        // Logic to add item to inventory
        Debug.Log($"Item added to inventory: {gameObject.name}");
        return true;
    }
}
