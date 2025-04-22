using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Special Item", menuName = "Inventory/Special Item")]
    public class SpecialItemDataSO : ItemDataSO
    {
        // public SpecialItemType specialItemType;
        public int level;

        private void OnValidate()
        {
            itemType = ItemType.Quest;
        }
    }
}