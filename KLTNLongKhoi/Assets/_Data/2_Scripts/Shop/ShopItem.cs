using UnityEngine;

namespace KLTNLongKhoi
{
    [System.Serializable]
    public class ShopItem
    {
        public string id;
        public ItemData item;
        public int price;
        public int stockQuantity; // -1 for unlimited
        public bool isAvailable = true;
        public Sprite icon;
        public string description;
        public ShopCategory category;
        public int levelRequirement; // Level required to buy this item
    }

    public enum ShopCategory
    {
        Weapons,
        Armor,
        Potions,
        Materials,
        Special
    }
}