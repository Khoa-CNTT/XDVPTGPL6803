using StarterAssets;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class InventoryOpener : MonoBehaviour
    { 
        public PopupScale inventoryUI; // Reference to the inventory UI GameObject

        private bool isOpen = false;
        
        StarterAssetsInputs inputs;

        void Start()
        {
            inputs = FindFirstObjectByType<StarterAssetsInputs>();

            inputs.openInventory.AddListener(OpenInventory);
        }

        void OpenInventory()
        {
            isOpen = !isOpen;
            
            if (isOpen)
            {
                inventoryUI.ScaleUp();
            }
            else
            {
                inventoryUI.ScaleDown();
            }
        }
    } 
}