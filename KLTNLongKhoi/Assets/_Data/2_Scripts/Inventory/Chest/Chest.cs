using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    //it's a chest for storing items
    public class Chest : MonoBehaviour, IInteractable
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

        public string GetInteractionText()
        {
            throw new System.NotImplementedException();
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public bool CanInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}