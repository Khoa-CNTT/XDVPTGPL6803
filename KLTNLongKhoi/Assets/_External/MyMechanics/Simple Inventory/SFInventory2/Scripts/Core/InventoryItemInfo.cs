using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Parity.SFInventory2.Core
{
    //Một lớp hiển thị thông tin về một vật phẩm. Bạn có thể triển khai IPointerEnterHandler để hiển thị thông tin về một vật phẩm khi bạn di chuột qua ô.
    public class InventoryItemInfo : MonoBehaviour
    {
        [SerializeField] private CellsCallbacksController _callbacksController;

        [SerializeField] private RectTransform _infoPanel;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _icon;

        private void Start()
        {
            _infoPanel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _callbacksController.onClick += OnClick;
            _callbacksController.onBeginDrag += OnBeginDrag;
        }

        private void OnDisable()
        {
            _callbacksController.onClick -= OnClick;
            _callbacksController.onBeginDrag -= OnBeginDrag;
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