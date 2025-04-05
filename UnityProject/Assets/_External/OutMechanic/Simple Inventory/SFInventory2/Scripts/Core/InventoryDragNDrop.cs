using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Parity.SFInventory2.Core
{
    // lớp xử lý kéo và chia đồ vật
    public class InventoryDragNDrop : MonoBehaviour
    {
        [SerializeField] private CellsCallbacksController _callbacksController;
        [SerializeField] private Image _dragIcon;

        private InventoryCell _dragCell;

        //Event Subscription
        private void OnEnable()
        {
            _callbacksController.onDrop += OnDrop;
            _callbacksController.onDrag += OnDrag;
            _callbacksController.onBeginDrag += OnBeginDrag;
            _callbacksController.onEndDrag += OnEndDrag;
        }

        private void OnDisable()
        {
            _callbacksController.onDrop -= OnDrop;
            _callbacksController.onDrag -= OnDrag;
            _callbacksController.onBeginDrag -= OnBeginDrag;
            _callbacksController.onEndDrag -= OnEndDrag;
        }

        public void StopDragging()
        {
            _dragIcon.gameObject.SetActive(false);
            _dragCell = null;
        }

        private void OnDrag(InventoryCell cell, PointerEventData eventData)
        {
            _dragIcon.transform.position = eventData.position;
        }

        // phương thức được gọi khi một ô được thả vào ô khác
        private void OnDrop(InventoryCell cell, PointerEventData eventData)
        {
            // kiểm tra xem ô kéo có tồn tại và khác với ô thả hay không
            if (_dragCell != null && cell != null && _dragCell != cell)
            {
                // kiểm tra xem ô thả có thể chứa đồ vật từ ô kéo hay không
                if (!cell.CanBeDropped(_dragCell))
                    return;

                // kiểm tra xem nút nhấn có phải là số hay không
                if (eventData.button == PointerEventData.InputButton.Middle)
                {
                    // kiểm tra xem ô thả có chứa đồ vật hay không
                    if (cell.Item == null)
                    {
                        SplitItems(cell, _dragCell);
                    }
                    else if (cell.Item == _dragCell.Item)
                    {
                        MoveItemsCount(cell, _dragCell, _dragCell.ItemsCount / 2);
                    }
                }
                // kiểm tra xem ô thả có chứa đồ vật khác ô kéo hay không
                else if (cell.Item != _dragCell.Item)
                {
                    SwapCells(cell, _dragCell);
                }
                // kiểm tra xem ô thả có chứa đồ vật giống ô kéo hay không
                else if (cell.Item == _dragCell.Item)
                {
                    MoveItemsCount(cell, _dragCell, _dragCell.ItemsCount);
                }
                // cập nhật giao diện ô thả và ô kéo
                cell.UpdateCellUI();
                _dragCell.UpdateCellUI();
            }
        }

        // phương thức chia đồ vật
        private void SplitItems(InventoryCell cell1, InventoryCell cell2)
        {
            var half = cell2.ItemsCount / 2;
            if (half > 0)
            {
                cell1.SetInventoryItem(cell2.Item);
                cell1.ItemsCount += half;
                cell2.ItemsCount -= half;
            }
        }

        // phương thức di chuyển số lượng đồ vật
        private void MoveItemsCount(InventoryCell cell1, InventoryCell cell2, int count)
        {
            if (count == 0)
                return;
            if (cell1.ItemsCount + count <= cell1.Item.maxItemsCount)
            {
                cell1.ItemsCount += count;
                cell2.ItemsCount -= count;
                if (cell2.ItemsCount <= 0)
                    cell2.SetInventoryItem(null);
            }
            else
            {
                if (cell1.ItemsCount < cell1.Item.maxItemsCount)
                {
                    cell2.ItemsCount -= (cell1.Item.maxItemsCount - cell1.ItemsCount);
                    cell1.ItemsCount += (cell1.Item.maxItemsCount - cell1.ItemsCount);
                }
            }
        }

        private void SwapCells(InventoryCell cell1, InventoryCell cell2)
        {
            var itemsSwap = (cell1.Item, cell2.Item);
            var itemsCountSwap = (cell1.ItemsCount, cell2.ItemsCount);

            cell1.SetInventoryItem(itemsSwap.Item2);
            cell2.SetInventoryItem(itemsSwap.Item1);

            cell1.ItemsCount = itemsCountSwap.Item2;
            cell2.ItemsCount = itemsCountSwap.Item1;
        }

        private void OnBeginDrag(InventoryCell cell, PointerEventData eventData)
        {
            if (cell.Item == null)
                return;
            _dragIcon.sprite = cell.Icon.sprite;
            _dragIcon.gameObject.SetActive(true);
            _dragIcon.transform.position = cell.Icon.transform.position;
            _dragCell = cell;
        }

        private void OnEndDrag(InventoryCell cell, PointerEventData eventData)
        {
            _dragCell = null;
            _dragIcon.gameObject.SetActive(false);
        }
    }
}