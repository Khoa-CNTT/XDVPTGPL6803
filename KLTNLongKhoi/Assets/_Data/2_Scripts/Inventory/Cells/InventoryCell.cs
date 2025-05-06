using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    //cell for storing and displaying items
    public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private bool isDraggable = true;
        [SerializeField] private Image icon;
        [SerializeField] private Image highlight; // Hightlight when selected
        [SerializeField] private TextMeshProUGUI itemsCountText;

        public Image Icon
        {
            get
            {
                return icon;
            }
        }
        public ItemDataSO ItemDataSO
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

        public Image Highlight { get => highlight; set => highlight = value; }

        private int _itemsCount;
        private ItemDataSO _item;
        private ContainerBase _container;

        public void Init(ContainerBase container)
        {
            _container = container;
            UpdateCellUI();
        }

        public void SetInventoryItem(ItemDataSO item)
        {
            _item = item;
        }

        public void UpdateCellUI()
        {
            if (_item != null)
            {
                icon.color = Color.white;
                icon.sprite = _item.icon;
            }
            else
            {
                _itemsCount = 0;
                icon.color = Color.clear;
                icon.sprite = null;
            }
            if (_itemsCount > 1)
                itemsCountText.text = "x" + ItemsCount;
            else
                itemsCountText.text = string.Empty;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isDraggable) return;
            _container.onBeginDrag?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDraggable) return;
            _container.onDrag?.Invoke(this, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            _container.onDrop?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDraggable) return;
            _container.onEndDrag?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _container.onClick?.Invoke(this, eventData);
        }

        internal void MigrateCell(ItemDataSO item)
        {
            SetInventoryItem(item);
        }

        public virtual bool CanBeDropped(InventoryCell cell)
        {
            return true;
        }
    }

}