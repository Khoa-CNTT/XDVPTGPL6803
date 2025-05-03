using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryData", menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<InventorySlot> defaultInventory = new List<InventorySlot>(); // Inventory mặc định
}
