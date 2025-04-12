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
        private List<StorageItem> _storageItems = new List<StorageItem>();

        public void ProceedStorage(StorageController storageController)
        {
            storageController.OpenStorage(this);
        }

        public void SaveItems(List<StorageItem> storageItems)
        {
            _storageItems = storageItems;
            //here you can save the items to a file
        }

        internal List<StorageItem> GetCells()
        {
            return _storageItems;
        }
    }
}