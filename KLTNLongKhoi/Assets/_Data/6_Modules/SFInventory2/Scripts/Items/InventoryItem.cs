using UnityEngine;

namespace Parity.SFInventory2.Core
{
    //basic inventory item
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/InventoryItems/Item", order = 1)]
    public class InventoryItem : ScriptableObject
    {
        public Sprite icon;
        public string itemName;
        public string itemDescription;
        public int maxItemsCount = 8;
    }
}