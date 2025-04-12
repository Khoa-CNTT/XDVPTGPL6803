using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Parity.SFInventory2.Core
{
    //it is the base class for the any type of inventory, where the logic resides
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
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].Init(this);
            }
        }

        public void AddInventoryCell(InventoryCell cell)
        {
            inventoryCells.Add(cell);
        }

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