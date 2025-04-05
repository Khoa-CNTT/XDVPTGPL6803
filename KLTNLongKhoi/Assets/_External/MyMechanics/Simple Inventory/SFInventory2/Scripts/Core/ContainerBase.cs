using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Parity.SFInventory2.Core
{
    //Đây là lớp cơ sở cho bất kỳ loại kho hàng nào, nơi logic nằm
    public class ContainerBase : MonoBehaviour
    {
        public List<InventoryCell> inventoryCells = new List<InventoryCell>();

        [SerializeField] private CellsCallbacksController _callbacksController;

        public Action<InventoryCell, PointerEventData> onBeginDrag => _callbacksController.onBeginDrag;
        public Action<InventoryCell, PointerEventData> onDrag => _callbacksController.onDrag;
        public Action<InventoryCell, PointerEventData> onEndDrag => _callbacksController.onEndDrag;
        public Action<InventoryCell, PointerEventData> onDrop => _callbacksController.onDrop;
        public Action<InventoryCell, PointerEventData> onClick => _callbacksController.onClick;

        private void Awake()
        {
            // khởi tạo ô đồ vật
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].Init(this);
            }
        }

        // thêm ô đồ vật vào kho
        public void AddInventoryCell(InventoryCell cell)
        {
            inventoryCells.Add(cell);
        }

        // cố gắng lấy ô trống
        public bool TryGetEmptyCell(out InventoryCell cell)
        {
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                if (inventoryCells[i].Item == null)
                {
                    cell = inventoryCells[i];
                    return true;
                }
            }
            cell = null;
            return false;
        }

        // cố gắng lấy ô có số lượng đồ vật tự do
        public bool TryGetCellWithFreeItemsCount(InventoryItem item, out InventoryCell cell)
        {
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                if (inventoryCells[i].Item == item)
                {
                    if (inventoryCells[i].ItemsCount < item.maxItemsCount)
                    {
                        cell = inventoryCells[i];
                        return true;
                    }
                }
            }
            cell = null;
            return false;
        }

        // thêm số lượng đồ vật
        public void AddItemsCount(InventoryItem item, int count, out int countLeft)
        {
            while (count > 0)
            {
                if (TryGetCellWithFreeItemsCount(item, out var cell))
                {
                    if ((cell.ItemsCount + count) > item.maxItemsCount)
                    {
                        count -= (item.maxItemsCount - cell.ItemsCount);
                        cell.ItemsCount = item.maxItemsCount;
                    }
                    else
                    {
                        cell.ItemsCount += count;
                        count = 0;
                    }
                    cell.UpdateCellUI();
                }
                else if (TryGetEmptyCell(out cell))
                {
                    cell.SetInventoryItem(item);
                    if (count > item.maxItemsCount)
                    {
                        cell.ItemsCount = item.maxItemsCount;
                        count -= item.maxItemsCount;
                    }
                    else
                    {
                        cell.ItemsCount += count;
                        count = 0;
                    }
                    cell.UpdateCellUI();
                }
                else
                {
                    break;
                }
            }
            countLeft = count;
        }
    }
}