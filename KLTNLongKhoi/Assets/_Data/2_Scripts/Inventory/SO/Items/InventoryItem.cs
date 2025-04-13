using UnityEngine;

namespace KLTNLongKhoi
{
    //basic inventory item
    [CreateAssetMenu(fileName = "Item", menuName = "KLTNLongKhoi/Item", order = 1)]
    public class InventoryItem : ScriptableObject
    {
        public Sprite icon;
        public string itemName;
        public string itemDescription;
        public int maxItemsCount = 8;
    }
}