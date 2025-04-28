using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KLTNLongKhoi
{
    //A class that displays information about an item. You can implement IPointerEnterHandler to display information about an item when you hover mouse over cell.
    public class InventoryItemInfo : MonoBehaviour
    {
        private CellsCallbacksController callbacksController;
        private ItemDataSO itemDataSO;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _icon;

        public ItemDataSO ItemDataSO { get => itemDataSO; set => itemDataSO = value; }

        private void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        private void OnEnable()
        {
            callbacksController.onClick += OnClick;
        }

        private void OnDisable()
        {
            callbacksController.onClick -= OnClick;
        }

        private void OnClick(InventoryCell cell, PointerEventData eventData)
        {
            Logic(cell);
        }

        private void Logic(InventoryCell cell)
        {
            if (_itemName == null || _itemDescription == null || _icon == null) return;
            if (cell != null && cell.ItemDataSO != null)
            {
                ItemDataSO = cell.ItemDataSO;
                _icon.sprite = cell.ItemDataSO.icon;
                _itemName.text = cell.ItemDataSO.itemData.name;
                _itemDescription.text = cell.ItemDataSO.itemData.description;
            }
        }
    }
}