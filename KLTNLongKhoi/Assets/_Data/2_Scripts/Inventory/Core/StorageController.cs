using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace KLTNLongKhoi
{
    //just an example of storage 
    public class StorageController : ContainerBase
    {
        [Header("StorageController")]
        [SerializeField] private RectTransform _storage;
        [SerializeField] private TextMeshProUGUI _storageName;
        private InventoryOpener inventoryOpener;
        private Chest currentChest;
        public Chest CurrentChest => currentChest;

        protected override void Awake()
        {
            base.Awake();
            inventoryOpener = FindFirstObjectByType<InventoryOpener>();
        }

        protected override void Start()
        {
            base.Start();
            _storage.gameObject.SetActive(false);
        }

        public void OpenStorage(Chest chest)
        {
            inventoryOpener.OpenInventory(true);

            SaveToChest();

            currentChest = chest;

            _storageName.text = currentChest.ChestName;

            var cells = currentChest.GetCells();

            if (cells != null)
            {
                _storage.gameObject.SetActive(true);
                if (cells.Count > 0)
                {
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].MigrateCell(cells[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].SetInventoryItem(null);
                    }
                }
            }
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].UpdateCellUI();
            }
        }

        public void CloseStorage()
        {
            inventoryOpener.OpenInventory(false);

            SaveToChest();
            currentChest = null;

            Invoke(nameof(DisableStorageUI), 0.6f);
        }

        private void DisableStorageUI()
        {
            _storage.gameObject.SetActive(false);
        }

        public void SaveToChest()
        {
            if (currentChest != null)
            {
                if (inventoryCells.Count > 0)
                {
                    currentChest.SaveItems(inventoryCells.Select(s => new StorageItem
                    {
                        item = s.Item,
                        itemsCount = s.ItemsCount
                    }).ToList());
                }
                else
                {
                    currentChest.SaveItems(new List<StorageItem>());
                }
            }
        }
    }
}
