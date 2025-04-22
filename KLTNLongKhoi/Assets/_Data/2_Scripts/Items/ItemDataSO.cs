using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemID;
        public string itemName;
        public ItemType itemType;
        public Sprite icon;
        public GameObject prefab;
        [TextArea(3, 10)] public string description;

        [Header("Requirements")]
        public int levelRequirement;
        public int strengthRequirement;
        public int intelligenceRequirement;

        [Header("Stats")]
        public ItemStats stats;

        [Header("Market")]
        public int buyPrice;
        public int sellPrice;
        public bool isSellable = true;

        [Header("Stacking")]
        public bool isStackable;
        public int maxStack = 1;
    }
}