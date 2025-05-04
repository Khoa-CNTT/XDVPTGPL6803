using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KLTNLongKhoi
{
    public class InventoryDataContact : MonoBehaviour
    {
        private SaveLoadManager saveLoadManager;
        private PlayerData playerData;

        public PlayerData PlayerData { get => playerData; set => playerData = value; }

        private void Awake()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
        }

        private void Start()
        {
            PlayerData = saveLoadManager.GetGameData().player;
            saveLoadManager.OnLoaded += () => PlayerData = saveLoadManager.GetGameData().player;
        }

        public bool TrySellItem(ItemData itemData, int count = 1)
        {
            var item = PlayerData.inventory.FirstOrDefault(i => i.name == itemData.name);
            if (item != null && item.itemCount >= count)
            {
                item.itemCount -= count;
                if (item.itemCount <= 0)
                {
                    PlayerData.inventory.Remove(item);
                }
                PlayerData.money += itemData.price * count;
                SaveInventory();
                return true;
            }
            return false;
        }

        public bool AddItem(ItemData itemData)
        {
            // Check if similar item exists and can be stacked
            var existingItem = PlayerData.inventory.FirstOrDefault(i => i.name == itemData.name);

            if (existingItem != null && existingItem.itemCount < existingItem.maxStack)
            {
                // Calculate how many items can be added to existing stack
                int spaceInStack = existingItem.maxStack - existingItem.itemCount;
                int itemsToAdd = Mathf.Min(itemData.itemCount, spaceInStack);

                existingItem.itemCount += itemsToAdd;
                itemData.itemCount -= itemsToAdd;

                // If there are remaining items and inventory isn't full, create new stack
                if (itemData.itemCount > 0 && PlayerData.inventory.Count < PlayerData.maxInventorySize)
                {
                    var newItem = new ItemData
                    {
                        name = itemData.name,
                        itemCount = Mathf.Min(itemData.itemCount, itemData.maxStack),
                        maxStack = itemData.maxStack
                    };
                    PlayerData.inventory.Add(newItem);
                }
            }
            // If no existing item found and inventory isn't full
            else if (PlayerData.inventory.Count < PlayerData.maxInventorySize)
            {
                var newItem = new ItemData
                {
                    name = itemData.name,
                    itemCount = Mathf.Min(itemData.itemCount, itemData.maxStack),
                    maxStack = itemData.maxStack
                };
                PlayerData.inventory.Add(newItem);
            }
            else
            {
                // Inventory is full
                SaveInventory();
                return false;
            }

            SaveInventory();
            return true;
        }

        public void RemoveItem(string itemName, int count = 1)
        {
            var item = PlayerData.inventory.FirstOrDefault(i => i.name == itemName);

            if (item != null)
            {
                item.itemCount -= count;
                if (item.itemCount <= 0)
                {
                    PlayerData.inventory.Remove(item);
                }
                SaveInventory();
            }
        }

        public void UpdateItem(ItemData updatedItem)
        {
            var existingItem = PlayerData.inventory.FirstOrDefault(i => i.name == updatedItem.name);

            if (existingItem != null)
            {
                int index = PlayerData.inventory.IndexOf(existingItem);
                PlayerData.inventory[index] = updatedItem;
                SaveInventory();
            }
        }

        public List<ItemData> GetInventory()
        {
            return PlayerData.inventory;
        }

        public ItemData GetItem(string itemName)
        {
            return PlayerData.inventory.FirstOrDefault(i => i.name == itemName);
        }

        private void SaveInventory()
        {
            saveLoadManager.SaveData(PlayerData);
        }

        // hàm kiểm tra nếu thêm item mới vào thì có thành công không chứ không phải là thêm trược tiếp vào
        public bool CheckAddItem(ItemData itemData)
        {
            var existingItem = PlayerData.inventory.FirstOrDefault(i => i.name == itemData.name);
            if (existingItem != null && existingItem.itemCount < existingItem.maxStack)
            {
                return true;
            }
            else if (existingItem == null && PlayerData.inventory.Count < PlayerData.maxInventorySize)
            {
                return true;
            }
            return false;
        }
    }
}
