using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "KLTNLongKhoi/Upgrade Recipe")]
    public class UpgradeRecipe : ScriptableObject
    {
        public InventoryItem baseWeapon;        // Vũ khí cần nâng cấp
        public InventoryItem upgradeMaterial;   // Nguyên liệu để nâng cấp
        public InventoryItem resultWeapon;      // Vũ khí sau khi nâng cấp
        public float upgradeChance = 0.7f;      // Tỷ lệ thành công (70% mặc định)
        [TextArea(3,5)]
        public string description;
    }
}