// ArmorDataSO.cs
using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
    public class ArmorDataSO : ItemDataSO
    {
        public ArmorType armorType;
        public int level; // Dùng cho Long Huyết (Sơ cấp, Trung cấp)

        private void OnValidate()
        {
            itemType = ItemType.Armor;
        }
    }
}