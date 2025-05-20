using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Parity.SFInventory2.Core
{
    //just an example of storage 
    public class StorageController : ContainerBase
    {
        [SerializeField] private RectTransform _storage;
        [SerializeField] private TextMeshProUGUI _storageName;
        public Chest CurrentChest
        {
            get
            {
                return _currentChest;
            }
        }
        private Chest _currentChest;

        private void Start()
        {
            _storage.gameObject.SetActive(false);
        }

        public void OpenStorage(Chest chest)
        {
            SaveToChest();

            _currentChest = chest;

            _storageName.text = _currentChest.ChestName;

            var cells = _currentChest.GetCells();

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
            SaveToChest();
            _currentChest = null;
            _storage.gameObject.SetActive(false);
        }

        public void SaveToChest()
        {
            if (_currentChest != null)
            {
                if (inventoryCells.Count > 0)
                {
                    _currentChest.SaveItems(inventoryCells.Select(s => new StorageItem
                    {
                        item = s.Item,
                        itemsCount = s.ItemsCount
                    }).ToList());
                }
                else
                {
                    _currentChest.SaveItems(new List<StorageItem>());
                }
            }
        }
    }
}
