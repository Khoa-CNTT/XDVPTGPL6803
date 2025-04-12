using UnityEngine;

namespace KLTNLongKhoi
{
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionText = "Press E to pickup";
        [SerializeField] private bool isInteractable = true;
        private PlayerInventory playerInventoryTest;

        void Start()
        {
            playerInventoryTest = FindFirstObjectByType<PlayerInventory>();
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
            // Add to inventory first
            if (playerInventoryTest != null && playerInventoryTest.TryAddItem(gameObject))
            {
                isInteractable = false;
                Destroy(gameObject);
            } 
        }

    }
}
