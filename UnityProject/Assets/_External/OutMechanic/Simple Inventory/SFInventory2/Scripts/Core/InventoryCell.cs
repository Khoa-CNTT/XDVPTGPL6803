using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Parity.SFInventory2.Core
{
    // Ô lưu trữ và hiển thị vật phẩm
    public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _itemsCountText;

        public Image Icon
        {
            get
            {
                return _icon;
            }
        }
        public InventoryItem Item
        {
            get
            {
                return _item;
            }
        }
        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value;
            }
        }

        private int _itemsCount;
        private InventoryItem _item;
        private ContainerBase _container;

        public void Init(ContainerBase container)
        {
            _container = container;
            UpdateCellUI();
        }

        public void SetInventoryItem(InventoryItem item)
        {
            _item = item;
        }

        public void UpdateCellUI()
        {
            // hiển thị icon vật phẩm
            if (_item != null)
            {
                _icon.color = Color.white;
                _icon.sprite = _item.icon;
            }
            else
            {
                _itemsCount = 0;
                _icon.color = Color.clear;
                _icon.sprite = null;
            }
            // hiển thị số lượng vật phẩm
            if (_itemsCount > 1)
                _itemsCountText.text = "x" + ItemsCount;
            else
                _itemsCountText.text = string.Empty;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _container.onBeginDrag?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _container.onDrag?.Invoke(this, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            _container.onDrop?.Invoke(this, eventData);
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            _container.onEndDrag?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _container.onClick?.Invoke(this, eventData);
        }

        internal void MigrateCell(StorageItem item)
        {
            _itemsCount = item.itemsCount;
            SetInventoryItem(item.item);
        }

        public virtual bool CanBeDropped(InventoryCell cell)
        {
            return true;
        }
    }

}