using UnityEngine;

namespace KLTNLongKhoi
{
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionText = "Press E to pickup";
        [SerializeField] private bool isInteractable = true;
        [SerializeField] InventoryItem inventoryItem;
        private InventoryController inventoryController;

        void Start()
        {
            inventoryController = FindFirstObjectByType<InventoryController>();
        }

        public string GetInteractionText()
        {
            return interactionText;
        }

        public bool CanInteract()
        {
            return isInteractable;
        }

        public void Interact()
        {
            if (inventoryItem == null)
            {
                Debug.LogWarning("No inventory item assigned to this interactable item!");
                return;
            }

            // Add to inventory first
            inventoryController.AddItemsCount(inventoryItem, inventoryItem.maxItemsCount, out var itemsLeft);
            if (itemsLeft > 0)
            {
                Debug.Log($"Not enough space! {itemsLeft} items left!");
            }
            else
            {
                isInteractable = false;
                Destroy(gameObject);
            }
        }
    }
}

