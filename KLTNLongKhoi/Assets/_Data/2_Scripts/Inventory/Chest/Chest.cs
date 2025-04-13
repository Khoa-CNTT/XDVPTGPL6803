using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    //it's a chest for storing items
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionText = "Press E to open/close";
        [SerializeField] private string chestName;
        private List<StorageItem> storageItems = new List<StorageItem>();
        private bool isInteractable = true;

        StorageController storageController;

        public string ChestName => chestName;

        void Awake()
        {
            storageController = FindFirstObjectByType<StorageController>();
        }

        public void SaveItems(List<StorageItem> storageItems)
        {
            this.storageItems = storageItems;
            //here you can save the items to a file
        }

        internal List<StorageItem> GetCells()
        {
            return storageItems;
        }

        public string GetInteractionText()
        {
            return interactionText;
        }

        public void Interact()
        {
            if (storageController.CurrentChest == this)
            {
                storageController.CloseStorage();
            }
            else
            {
                storageController.OpenStorage(this);
            }
        }

        public bool CanInteract()
        {
            return isInteractable;
        }
    }
}