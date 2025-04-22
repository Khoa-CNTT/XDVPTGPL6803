using UnityEngine;

namespace KLTNLongKhoi
{
    //basic inventory item
    [CreateAssetMenu(fileName = "Item", menuName = "KLTNLongKhoi/Item", order = 1)]
    public class InventoryItemSO : ScriptableObject
    {
        public Sprite icon;
        public ItemDataSO itemData; // dữ liệu của item
        public string itemName;
        public string itemDescription;
        public int maxItemsCount = 8;
    }
}