using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public class ShopUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private TMP_Text _playerCurrencyText;
        [SerializeField] private TMP_Text _itemDescriptionText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private TMP_InputField _quantityInput;
        
        [Header("Category Filters")]
        [SerializeField] private List<Toggle> _categoryToggles;
        
        private ShopManager _shopManager;
        private ShopItem _selectedItem;
        private int _quantity = 1;

        private void Start()
        {
            _shopManager = ShopManager.Instance;
            _shopManager.OnTransactionComplete += OnTransactionComplete;
            
            _buyButton.onClick.AddListener(OnBuyClicked);
            _sellButton.onClick.AddListener(OnSellClicked);
            _quantityInput.onValueChanged.AddListener(OnQuantityChanged);
            
            RefreshUI();
        }

        private void OnBuyClicked()
        {
            if (_selectedItem == null) return;
            if (_shopManager.TryBuyItem(_selectedItem.id, _quantity))
            {
                RefreshUI();
            }
        }

        private void OnSellClicked()
        {
            if (_selectedItem == null) return;
            if (_shopManager.TrySellItem(_selectedItem.item, _quantity))
            {
                RefreshUI();
            }
        }

        private void OnQuantityChanged(string value)
        {
            if (int.TryParse(value, out int result))
            {
                _quantity = Mathf.Max(1, result);
                UpdatePriceDisplay();
            }
        }

        private void OnTransactionComplete(ShopItem item, int quantity, bool isBuying)
        {
            RefreshUI();
            // Add visual/audio feedback here
        }

        private void RefreshUI()
        {
            // Update currency display
            // Refresh item list
            // Update selected item info
        }

        private void UpdatePriceDisplay()
        {
            // Update buy/sell price based on selected item and quantity
        }

        private void OnDestroy()
        {
            if (_shopManager != null)
            {
                _shopManager.OnTransactionComplete -= OnTransactionComplete;
            }
        }
    }
}