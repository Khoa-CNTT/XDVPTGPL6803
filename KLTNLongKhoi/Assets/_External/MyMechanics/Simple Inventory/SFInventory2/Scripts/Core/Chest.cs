using System.Collections.Generic;
using UnityEngine;

namespace Parity.SFInventory2.Core
{
    //it's a chest for storing items
    public class Chest : MonoBehaviour
    {
        public string ChestName
        {
            get
            {
                return chestName;
            }
        }
        [SerializeField] private string chestName;
        [SerializeField] List<StorageItem> _storageItems = new List<StorageItem>();

        // xử lý kho
        public void ProceedStorage(StorageController storageController)
        {
            storageController.OpenStorage(this);
        }

        // lưu trữ đồ vật
        public void SaveItems(List<StorageItem> storageItems)
        {
            _storageItems = storageItems;
            //here you can save the items to a file
        }

        // lấy ô đồ vật
        internal List<StorageItem> GetCells()
        {
            return _storageItems;
        }
    }
}