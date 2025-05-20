using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    //it's a chest for storing items
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionText = "Press E to open/close";
        [SerializeField] private string chestName;
        [SerializeField] private List<ItemDataSO> storageItems = new List<ItemDataSO>();
        private bool isInteractable = true;

        StorageController storageController;

        public string ChestName => chestName;

        public List<ItemDataSO> StorageItems { get => storageItems; set => storageItems = value; }

        void Awake()
        {
            storageController = FindFirstObjectByType<StorageController>();
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