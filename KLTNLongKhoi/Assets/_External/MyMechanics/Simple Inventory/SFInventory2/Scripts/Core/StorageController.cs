using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Parity.SFInventory2.Core
{
    // một ví dụ về kho đồ vật
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

        // mở kho
        public void OpenStorage(Chest chest)
        {
            SaveToChest();

            _currentChest = chest;

            _storageName.text = _currentChest.ChestName;

            var cells = _currentChest.GetCells();

            // nếu có kho thì mở kho
            if (cells != null)
            {
                _storage.gameObject.SetActive(true);
                if (cells.Count > 0)
                {
                    // nếu có vật phẩm thì di chuyển vật phẩm từ kho hiện tại sang kho mới
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].MigrateCell(cells[i]);
                    }
                }
                else
                {
                    // nếu không có vật phẩm thì đặt vật phẩm là null
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].SetInventoryItem(null);
                    }
                }
            }
            // cập nhật lại UI
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].UpdateCellUI();
            }
        }

        // đóng kho
        public void CloseStorage()
        {
            SaveToChest();
            _currentChest = null;
            _storage.gameObject.SetActive(false);
        }

        // lưu trữ đồ vật vào kho
        public void SaveToChest()
        {
            if (_currentChest != null)
            {
                if (inventoryCells.Count > 0)
                {
                    // lưu trữ đồ vật vào kho
                    _currentChest.SaveItems(inventoryCells.Select(s => new StorageItem
                    {
                        item = s.Item,
                        itemsCount = s.ItemsCount
                    }).ToList());
                }
                else
                {
                    // nếu không có vật phẩm thì đặt vật phẩm là null
                    _currentChest.SaveItems(new List<StorageItem>());
                }
            }
        }
    }
}
