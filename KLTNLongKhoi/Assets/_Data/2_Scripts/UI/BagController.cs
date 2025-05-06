using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace KLTNLongKhoi
{
    public class BagController : MonoBehaviour
    {
        [SerializeField] TMP_Text notifySell;
        [SerializeField] Button btnClosePanel;
        [SerializeField] Button btnOpenBag;
        [SerializeField] Button btnOpenItemResource;
        [SerializeField] Button btnOpenItemWeapon;
        [SerializeField] Button sellItem;
        [SerializeField] Button btnOpenItemConsumable;
        [SerializeField] bool isOpen = false;
        [SerializeField] InventoryItemInfo inventoryItemInfo; // inventoryItemInfo của Bag
        private OnTriggerThis openPanel;
        private SaveLoadManager saveLoadManager;
        private PauseManager pauseManager;
        private ContainerBase containerBase;
        private InventoryDataContact inventoryDataContact;
        private ItemType itemTypeOpening;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            containerBase = GetComponent<ContainerBase>();
            btnOpenItemResource.onClick.AddListener(AddItemResourceToBag);
            btnOpenItemWeapon.onClick.AddListener(AddItemWeaponToBag);
            btnOpenItemConsumable.onClick.AddListener(AddItemConsumableToBag);
            inventoryDataContact = FindFirstObjectByType<InventoryDataContact>();
            openPanel = GetComponentInChildren<OnTriggerThis>();
        }

        void Start()
        {
            sellItem.onClick.AddListener(SellItem);
            btnClosePanel?.onClick.AddListener(CloseStorage);
            btnOpenBag?.onClick.AddListener(OpenStorage);
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
            openPanel.ActiveObjects();
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
            openPanel.UnActiveObjects();
            pauseManager?.ResumeGame();
            isOpen = false;
        }

        private void SellItem()
        {
            if (inventoryItemInfo.ItemDataSO == null)
            {
                OnSellFail("Chọn vật phẩm để bán");
                return;
            }
            ItemData itemSelecting = inventoryItemInfo.ItemDataSO.itemData;

            bool isSell = inventoryDataContact.TrySellItem(itemSelecting);

            if (isSell)
            {
                OnSellScuccess("Bán thành công");
                OpenStorage();
            }
            else
            {
                OnSellFail("Xóa không thành công");
            }
        }

        private void OnSellScuccess(string message)
        {
            notifySell.gameObject.SetActive(true);
            notifySell.text = message;
            notifySell.color = Color.green;
            Invoke("HideNotify", 1f);
        }

        private void OnSellFail(string message)
        {
            notifySell.gameObject.SetActive(true);
            notifySell.text = message;
            notifySell.color = Color.red;
            Invoke("HideNotify", 1f);
        }

        private void HideNotify()
        {
            notifySell.gameObject.SetActive(false);
        }
    }
}