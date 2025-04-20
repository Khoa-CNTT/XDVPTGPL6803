using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class InventoryOpener : MonoBehaviour
    {
        [SerializeField] PopupScale inventoryUI;
        [SerializeField] private bool openAtStart;
        private bool isOpen = false;

        StarterAssetsInputs inputs;
        InventoryDragNDrop inventoryDragNDrop;
        PauseManager pauseManager;

        private void Start()
        {
            inputs = FindFirstObjectByType<StarterAssetsInputs>();
            inputs.openInventory.AddListener(OpenInventory);
            inventoryDragNDrop = FindFirstObjectByType<InventoryDragNDrop>();
            pauseManager = FindFirstObjectByType<PauseManager>();

            if (openAtStart == false)
            {
                inventoryUI.ScaleDown();
            }
        }

        private void OpenInventory()
        {
            if (pauseManager.IsPaused && isOpen == false)
            {
                return;
            }
            
            isOpen = !isOpen;
            inventoryDragNDrop.StopDragging();

            if (isOpen)
            {
                inventoryUI.ScaleUp();
            }
            else
            {
                inventoryUI.ScaleDown();
            }

            pauseManager.SetPause(isOpen);
        }

        public void OpenInventory(bool isOpen)
        {
            this.isOpen = isOpen;
            inventoryDragNDrop.StopDragging();
            
            if (isOpen)
            {
                inventoryUI.ScaleUp();
            }
            else
            {
                inventoryUI.ScaleDown();
            }
            pauseManager.SetPause(isOpen);
        }
    }
}