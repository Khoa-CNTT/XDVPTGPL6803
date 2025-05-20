using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace KLTNLongKhoi
{
    public class ShopUI : ContainerBase
    {
        [SerializeField] private AudioClip buyItemSuccess;
        [SerializeField] private List<ItemDataSO> _shopItemsEquipment; // Combined weapons and armor
        [SerializeField] private List<ItemDataSO> _shopItemsPotions;
        [SerializeField] private List<ItemDataSO> _shopItemsMaterials;
        [SerializeField] private TMP_Text _notifyBuySuccess;
        [SerializeField] private Button btnBuyEquipment;
        [SerializeField] private Button btnBuyPotion;
        [SerializeField] private Button btnBuyMaterial;
        [SerializeField] private Button btnBuy;
        [SerializeField] private Button btnOpenShop;
        [SerializeField] private Button btnCloseShop;
        [SerializeField] private OnTriggerThis openShop;
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
            btnCloseShop.onClick.AddListener(CloseStorage);
            openShop = GetComponentInChildren<OnTriggerThis>();
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

            openShop.ActiveObjects();
        }

        public void CloseStorage()
        {
            openShop.UnActiveObjects();
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
                                                .FirstOrDefault(x => x.ItemData.name == shopItem.name);

                if (itemDataSO != null)
                {
                    AddItemsCount(itemDataSO, itemDataSO.ItemData.maxStack, out var countLeft);
                    if (countLeft > 0)
                    {
                        Debug.Log($"Not enough space for {itemDataSO.ItemData.name}! {countLeft} items left!");
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
                if (inventoryDataContact.PlayerData.money >= inventoryItemInfo.ItemDataSO.ItemData.price)
                { 
                    if (inventoryDataContact.AddItem(inventoryItemInfo.ItemDataSO.ItemData))
                    {
                        inventoryDataContact.PlayerData.money -= inventoryItemInfo.ItemDataSO.ItemData.price;
                        OnBuySuccess();
                    }
                    else
                    {
                        OnBuyFail("Không đủ chỗ trong túi xách");
                    }
                }
                else
                {
                    OnBuyFail("Không đủ tiền");
                }
            }
        }

        private void OnBuySuccess()
        {
            GetComponent<AudioSource>().PlayOneShot(buyItemSuccess);
            _notifyBuySuccess.gameObject.SetActive(true);
            _notifyBuySuccess.text = "Mua thành công";
            _notifyBuySuccess.color = Color.green;
            Invoke("HideNotify", 2f);
        }

        private void OnBuyFail(string message)
        {
            _notifyBuySuccess.gameObject.SetActive(true);
            _notifyBuySuccess.text = message;
            _notifyBuySuccess.color = Color.red;
            Invoke("HideNotify", 2f);
        }

        private void HideNotify()
        {
            _notifyBuySuccess.gameObject.SetActive(false);
        }
    }
}
