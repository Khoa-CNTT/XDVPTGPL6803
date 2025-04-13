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

        [SerializeField] private RectTransform _infoPanel;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _icon;

        private void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        private void Start()
        {
            _infoPanel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            callbacksController.onClick += OnClick;
            callbacksController.onBeginDrag += OnBeginDrag;
        }

        private void OnDisable()
        {
            callbacksController.onClick -= OnClick;
            callbacksController.onBeginDrag -= OnBeginDrag;
        }

        private void OnClick(InventoryCell cell, PointerEventData eventData)
        {
            Logic(cell);
        }

        private void OnBeginDrag(InventoryCell cell, PointerEventData eventData)
        {
            Logic(null);
        }

        private void Logic(InventoryCell cell)
        {
            if (cell != null && cell.Item != null)
            {
                _infoPanel.gameObject.SetActive(true);
                _icon.sprite = cell.Item.icon;
                _itemName.text = cell.Item.itemName;
                _itemDescription.text = cell.Item.itemDescription;
            }
            else
            {
                _infoPanel.gameObject.SetActive(false);
            }
        }
    }
}