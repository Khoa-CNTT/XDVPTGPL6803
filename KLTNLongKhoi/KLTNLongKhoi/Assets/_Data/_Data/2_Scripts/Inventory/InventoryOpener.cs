using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace KLTNLongKhoi
{
    public class InventoryOpener : MonoBehaviour
    {
        [SerializeField] private bool isOpen = false;
        OnTriggerThis onTriggerThis;
        StarterAssetsInputs inputs;
        InventoryDragNDrop inventoryDragNDrop;
        PauseManager pauseManager;

        private void Start()
        {
            inputs = FindFirstObjectByType<StarterAssetsInputs>();
            inputs.openInventory += OpenInventory;
            inventoryDragNDrop = FindFirstObjectByType<InventoryDragNDrop>();
            pauseManager = FindFirstObjectByType<PauseManager>();
            onTriggerThis = GetComponentInChildren<OnTriggerThis>();
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
                onTriggerThis.ActiveObjects();
            }
            else
            {
                onTriggerThis.UnActiveObjects();
            }

            pauseManager.SetPause(isOpen);
        }

        public void OpenInventory(bool isOpen)
        {
            this.isOpen = isOpen;
            inventoryDragNDrop.StopDragging();

            if (isOpen)
            {
                onTriggerThis.ActiveObjects();
            }
            else
            {
                onTriggerThis.UnActiveObjects();
            }
            pauseManager.SetPause(isOpen);
        }
    }
}