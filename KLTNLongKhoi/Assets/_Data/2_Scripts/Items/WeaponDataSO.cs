using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    public class WeaponDataSO : ItemDataSO
    {
        public WeaponType weaponType;
        public int level;

        private void OnValidate()
        {
            itemType = ItemType.Weapon;
        }
    }
}