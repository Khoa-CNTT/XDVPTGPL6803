using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KLTNLongKhoi
{
    //it is the base class for the any type of inventory, where the logic resides
    public class ContainerBase : MonoBehaviour
    {
        public List<InventoryCell> inventoryCells;

        [SerializeField] private Transform cellsContainer;

        private CellsCallbacksController callbacksController;

        public Action<InventoryCell, PointerEventData> onBeginDrag => callbacksController.onBeginDrag;
        public Action<InventoryCell, PointerEventData> onDrag => callbacksController.onDrag;
        public Action<InventoryCell, PointerEventData> onEndDrag => callbacksController.onEndDrag;
        public Action<InventoryCell, PointerEventData> onDrop => callbacksController.onDrop;
        public Action<InventoryCell, PointerEventData> onClick => callbacksController.onClick;

        protected virtual void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        protected virtual void Start()
        {
            inventoryCells = new List<InventoryCell>(cellsContainer.GetComponentsInChildren<InventoryCell>());
            // nghĩa là nó sẽ tự động tạo ra các ô trong inventory
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].Init(this);
            }
        }

        public bool TryGetEmptyCell(out InventoryCell cell)
        {
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                if (inventoryCells[i].ItemDataSO == null)
                {
                    cell = inventoryCells[i];
                    return true;
                }
            }
            cell = null;
            return false;
        }

        // hàm để tìm ô trống trong inventory
        public bool TryGetCellWithFreeItemsCount(ItemDataSO item, out InventoryCell cell)
        {
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                if (inventoryCells[i].ItemDataSO == item)
                {
                    if (inventoryCells[i].ItemsCount < item.ItemData.maxStack)
                    {
                        cell = inventoryCells[i];
                        return true;
                    }
                }
            }
            cell = null;
            return false;
        }

        public void AddItemsCount(ItemDataSO item, int count, out int countLeft)
        {
            while (count > 0)
            {
                if (TryGetCellWithFreeItemsCount(item, out var cell))
                {
                    if ((cell.ItemsCount + count) > item.ItemData.maxStack)
                    {
                        count -= (item.ItemData.maxStack - cell.ItemsCount);
                        cell.ItemsCount = item.ItemData.maxStack;
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
                    if (count > item.ItemData.maxStack)
                    {
                        cell.ItemsCount = item.ItemData.maxStack;
                        count -= item.ItemData.maxStack;
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

            Debug.Log("Count left: " + count);
            countLeft = count;
            SaveInventory();
        }

        public void AddInventoryCell(InventoryCell cell)
        {
            inventoryCells.Add(cell);
        }

        public void RemoveInventoryCell(ItemDataSO item, int count, out int countLeft)
        {
            int remainingCount = count;
            bool itemFound = false;
            
            while (remainingCount > 0)
            {
                itemFound = false;
                for (int i = 0; i < inventoryCells.Count; i++)
                {
                    if (inventoryCells[i].ItemDataSO == item)
                    {
                        itemFound = true;
                        if (inventoryCells[i].ItemsCount > remainingCount)
                        {
                            inventoryCells[i].ItemsCount -= remainingCount;
                            remainingCount = 0;
                        }
                        else
                        {
                            remainingCount -= inventoryCells[i].ItemsCount;
                            inventoryCells[i].ItemsCount = 0;
                            inventoryCells[i].SetInventoryItem(null);
                        }
                        inventoryCells[i].UpdateCellUI();
                        break; // Xử lý một cell rồi thoát vòng lặp for
                    }
                }
                
                // Nếu không tìm thấy item nào trong inventory, thoát vòng lặp
                if (!itemFound) break;
            }
            
            SaveInventory();
            countLeft = remainingCount;
        }

        protected virtual void SaveInventory() { }
        protected virtual void LoadInventory() { }
    }
}
