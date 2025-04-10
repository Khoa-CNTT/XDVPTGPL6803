using UnityEngine;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public class ShopManager : Singleton<ShopManager>
    {
        [SerializeField] private List<ShopItem> _availableItems = new List<ShopItem>();
        [SerializeField] private float _buyMultiplier = 1f; // Price multiplier when buying
        [SerializeField] private float _sellMultiplier = 0.5f; // Price multiplier when selling
        
        private PlayerData _playerData;

        public delegate void ShopTransactionHandler(ShopItem item, int quantity, bool isBuying);
        public event ShopTransactionHandler OnTransactionComplete;

        public bool TryBuyItem(string itemId, int quantity = 1)
        {
            ShopItem shopItem = _availableItems.Find(x => x.id == itemId);
            if (shopItem == null || !CanBuyItem(shopItem, quantity))
                return false;

            int totalCost = CalculateBuyPrice(shopItem, quantity);
            _playerData.currency -= totalCost;
            
            // Add item to player inventory
            // You'll need to implement this based on your inventory system
            
            OnTransactionComplete?.Invoke(shopItem, quantity, true);
            return true;
        }

        public bool TrySellItem(ItemData item, int quantity = 1)
        {
            if (item == null) return false;

            int sellPrice = CalculateSellPrice(item, quantity);
            _playerData.currency += sellPrice;
            
            // Remove item from player inventory
            // You'll need to implement this based on your inventory system
            
            OnTransactionComplete?.Invoke(
                _availableItems.Find(x => x.item.id == item.id), 
                quantity, 
                false
            );
            return true;
        }

        private bool CanBuyItem(ShopItem item, int quantity)
        {
            if (!item.isAvailable) return false;
            if (item.stockQuantity != -1 && item.stockQuantity < quantity) return false;
            if (_playerData.level < item.levelRequirement) return false;
            if (_playerData.currency < CalculateBuyPrice(item, quantity)) return false;
            return true;
        }

        private int CalculateBuyPrice(ShopItem item, int quantity)
        {
            return Mathf.RoundToInt(item.price * quantity * _buyMultiplier);
        }

        private int CalculateSellPrice(ItemData item, int quantity)
        {
            ShopItem shopItem = _availableItems.Find(x => x.item.id == item.id);
            if (shopItem == null) return 0;
            return Mathf.RoundToInt(shopItem.price * quantity * _sellMultiplier);
        }

        public void SetPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
        }
    }
}