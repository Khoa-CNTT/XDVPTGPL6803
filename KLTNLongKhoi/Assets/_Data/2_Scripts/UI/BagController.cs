using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine.UI;
using System.Linq;

namespace KLTNLongKhoi
{
    public class BagController : MonoBehaviour
    {
        public UnityEvent openPanel;
        public UnityEvent closePanel;
        PauseManager pauseManager;
        [SerializeField] Button btnOpenItemResource;
        [SerializeField] Button btnOpenItemWeapon;
        [SerializeField] Button btnOpenItemConsumable;
        [SerializeField] bool isOpen = false;
        private SaveLoadManager saveLoadManager;
        private ContainerBase containerBase;
        private ItemType itemTypeOpening;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            containerBase = GetComponent<ContainerBase>();
            btnOpenItemResource.onClick.AddListener(AddItemResourceToBag);
            btnOpenItemWeapon.onClick.AddListener(AddItemWeaponToBag);
            btnOpenItemConsumable.onClick.AddListener(AddItemConsumableToBag);
        }

        private void Update()
        {
            if (pauseManager == null) return;
            if (pauseManager.IsPaused && isOpen == false)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    OpenStorage();
                }
                else
                {
                    CloseStorage();
                }
            }
        }

        // Clear hết item cell trong ContainerBase
        private void ClearInventoryCells()
        {
            foreach (var cell in containerBase.inventoryCells)
            {
                cell.SetInventoryItem(null);
                cell.ItemsCount = 0;
                cell.UpdateCellUI();
            }
        }

        // Thêm item vào ContainerBase nhưng chỉ cho item có type là resource vào
        public void AddItemTypeToBag(ItemType itemType)
        {
            List<ItemDataSO> resourceItems = GetPlayerInventoryItem();
            foreach (var item in resourceItems)
            {
                if (item.itemData.itemType == itemType)
                {
                    containerBase.AddItemsCount(item, item.itemData.maxStack, out var countLeft);
                }
            }
        }

        private List<ItemDataSO> GetPlayerInventoryItem()
        {
            List<ItemDataSO> resourceItems = new List<ItemDataSO>();

            foreach (var item in saveLoadManager.GetGameData().player.inventory)
            {
                ItemDataSO itemDataSO = Resources.LoadAll<ItemDataSO>("Items")
                                                .FirstOrDefault(x => x.itemData.name == item.name);
                resourceItems.Add(itemDataSO);
            }

            return resourceItems;   
        }

        public void AddItemResourceToBag()
        {
            ClearInventoryCells();
            AddItemTypeToBag(ItemType.Resource);
            itemTypeOpening = ItemType.Resource;
        }

        public void AddItemWeaponToBag()
        {
            ClearInventoryCells();
            AddItemTypeToBag(ItemType.Weapon);
            AddItemTypeToBag(ItemType.Armor);
            itemTypeOpening = ItemType.Weapon;
        }

        public void AddItemConsumableToBag()
        {
            ClearInventoryCells();
            AddItemTypeToBag(ItemType.Consumable);
            itemTypeOpening = ItemType.Consumable;
        }

        public void OpenStorage()
        {
            openPanel.Invoke();
            pauseManager?.PauseGame();
            isOpen = true;

            if (itemTypeOpening == ItemType.Resource)
            {
                AddItemResourceToBag();
            }
            else if (itemTypeOpening == ItemType.Weapon)
            {
                AddItemWeaponToBag();
            }
            else if (itemTypeOpening == ItemType.Consumable)
            {
                AddItemConsumableToBag();
            }
            else
            {
                AddItemTypeToBag(ItemType.Resource);
            }
        }

        public void CloseStorage()
        {
            closePanel.Invoke();
            pauseManager?.ResumeGame();
            isOpen = false;
        }
    }
}