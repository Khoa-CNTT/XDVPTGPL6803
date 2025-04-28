using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace KLTNLongKhoi
{
    public class ShopUI : ContainerBase
    {
        [SerializeField] private List<ItemDataSO> _shopItemsEquipment; // Combined weapons and armor
        [SerializeField] private List<ItemDataSO> _shopItemsPotions;
        [SerializeField] private List<ItemDataSO> _shopItemsMaterials;
        [SerializeField] private TMP_Text _notifyBuySuccess;
        [SerializeField] private Button btnBuyEquipment;
        [SerializeField] private Button btnBuyPotion;
        [SerializeField] private Button btnBuyMaterial;
        [SerializeField] private Button btnBuy;
        [SerializeField] private Button btnOpenShop;
        [SerializeField] private InventoryItemInfo inventoryItemInfo;
        private InventoryDataContact inventoryDataContact;
        private ItemType itemTypeOpening;

        protected override void Start()
        {
            base.Start();
            btnBuy.onClick.AddListener(OnBuyItem);
            btnBuyMaterial.onClick.AddListener(OnClickBuyMaterial);
            btnBuyPotion.onClick.AddListener(OnClickBuyPotion);
            btnBuyEquipment.onClick.AddListener(OnClickBuyEquipment);
            btnOpenShop.onClick.AddListener(OnClickOpenShop);
        }

        public void OnClickOpenShop()
        {
            if (itemTypeOpening == ItemType.Resource)
            {
                OnClickBuyMaterial();
            }
            else if (itemTypeOpening == ItemType.Weapon)
            {
                OnClickBuyEquipment();
            }
            else if (itemTypeOpening == ItemType.Consumable)
            {
                OnClickBuyPotion();
            }
            else
            {
                OnClickBuyPotion();
            }
        }

        private void OnClickBuyEquipment()
        {
            DisplayShopItems(_shopItemsEquipment);
            itemTypeOpening = ItemType.Weapon;
        }

        private void OnClickBuyPotion()
        {
            DisplayShopItems(_shopItemsPotions);
            itemTypeOpening = ItemType.Consumable;
        }

        private void OnClickBuyMaterial()
        {
            DisplayShopItems(_shopItemsMaterials);
            itemTypeOpening = ItemType.Resource;
        }

        private void DisplayShopItems(List<ItemDataSO> shopItems)
        {
            ClearInventoryCells();

            foreach (var shopItem in shopItems)
            {

                ItemDataSO itemDataSO = Resources.LoadAll<ItemDataSO>("Items")
                                                .FirstOrDefault(x => x.itemData.name == shopItem.name);

                if (itemDataSO != null)
                {
                    AddItemsCount(itemDataSO, itemDataSO.itemData.maxStack, out var countLeft);
                    if (countLeft > 0)
                    {
                        Debug.Log($"Not enough space for {itemDataSO.itemData.name}! {countLeft} items left!");
                    }
                }
            }
        }

        private void ClearInventoryCells()
        {
            foreach (var cell in inventoryCells)
            {
                cell.SetInventoryItem(null);
                cell.ItemsCount = 0;
                cell.UpdateCellUI();
            }
        }

        private void OnBuyItem()
        {
            inventoryDataContact = FindFirstObjectByType<InventoryDataContact>();

            if (inventoryItemInfo.ItemDataSO != null)
            {
                if (inventoryDataContact.PlayerData.money >= inventoryItemInfo.ItemDataSO.itemData.price)
                { 
                    if (inventoryDataContact.AddItem(inventoryItemInfo.ItemDataSO.itemData))
                    {
                        inventoryDataContact.PlayerData.money -= inventoryItemInfo.ItemDataSO.itemData.price;
                        OnBuySuccess();
                    }
                    else
                    {
                        OnBuyFail();
                    }
                }
                else
                {
                    OnBuyFail();
                }
            }
        }

        private void OnBuySuccess()
        {
            _notifyBuySuccess.gameObject.SetActive(true);
            _notifyBuySuccess.text = "Mua thành công";
            _notifyBuySuccess.color = Color.green;
            Invoke("HideNotify", 2f);
        }

        private void OnBuyFail()
        {
            _notifyBuySuccess.gameObject.SetActive(true);
            _notifyBuySuccess.text = "Không đủ tiền";
            _notifyBuySuccess.color = Color.red;
            Invoke("HideNotify", 2f);
        }

        private void HideNotify()
        {
            _notifyBuySuccess.gameObject.SetActive(false);
        }
    }
}
