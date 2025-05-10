using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace KLTNLongKhoi
{
    public class InventoryController : ContainerBase
    {
        private SaveLoadManager saveLoadManager;


        protected override void Awake()
        {
            base.Awake();
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
        }

        protected override void Start()
        {
            base.Start();
            LoadInventory();
            saveLoadManager.OnLoaded += LoadInventory;
        }

        public void SaveThisInventory()
        {
            SaveInventory();
        }

        protected override void SaveInventory()
        {
            if (saveLoadManager != null)
            {
                var gameData = saveLoadManager.GetGameData();
                gameData.player.inventory.Clear();

                foreach (var cell in inventoryCells)
                {
                    if (cell.ItemDataSO != null)
                    {
                        gameData.player.inventory.Add(cell.ItemDataSO.ItemData);
                    }
                }

                saveLoadManager.SaveData(gameData.player);
            }
        }

        protected override void LoadInventory()
        {
            if (saveLoadManager != null)
            {
                var gameData = saveLoadManager.GetGameData();
                if (gameData.player.inventory != null && gameData.player.inventory.Count > 0)
                {
                    // Clear existing inventory first
                    foreach (var cell in inventoryCells)
                    {
                        cell.SetInventoryItem(null);
                        cell.ItemsCount = 0;
                    }

                    // Create a copy of the inventory before iterating
                    var inventoryCopy = new List<ItemData>(gameData.player.inventory);
                    
                    // Load saved inventory
                    foreach (var itemData in inventoryCopy)
                    {
                        if (itemData != null)
                        {
                            ItemDataSO item = Resources.LoadAll<ItemDataSO>("Items")
                                                  .FirstOrDefault(x => x.ItemData.name == itemData.name);
                            AddItemsCount(item, itemData.itemCount, out var countLeft);
                        }
                    }
                }
            }
        }

        // Optional: Add method to save inventory when game is paused or inventory is closed
        public void OnInventoryClose()
        {
            SaveInventory();
        }
    }
}
