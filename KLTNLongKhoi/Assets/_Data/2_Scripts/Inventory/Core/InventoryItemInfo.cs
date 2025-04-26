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
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _icon;

        private void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        private void OnEnable()
        {
            callbacksController.onClick += OnClick;
        }

        private void OnClick(InventoryCell cell, PointerEventData eventData)
        {
            Logic(cell);
        }

        private void Logic(InventoryCell cell)
        {
            if (_itemName == null || _itemDescription == null || _icon == null) return;
            if (cell != null && cell.Item != null)
            {
                _icon.sprite = cell.Item.icon;
                _itemName.text = cell.Item.itemName;
                _itemDescription.text = cell.Item.itemDescription;
            }
        }
    }
}