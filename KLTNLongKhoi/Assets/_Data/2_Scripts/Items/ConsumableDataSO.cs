using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
    public class ConsumableDataSO : ItemDataSO
    {
        public ConsumableType consumableType;
        public int level; // Dùng cho Bình máu các cấp

        private void OnValidate()
        {
            itemType = ItemType.Consumable;
        }
    }
}