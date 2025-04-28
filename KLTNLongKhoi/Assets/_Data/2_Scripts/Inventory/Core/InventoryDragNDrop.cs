using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KLTNLongKhoi
{
    //class for handling dragging and dividing objects
    public class InventoryDragNDrop : MonoBehaviour
    {
        private CellsCallbacksController callbacksController;
        [SerializeField] private Image _dragIcon;

        private InventoryCell _dragCell;

        private void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        private void OnEnable()
        {
            callbacksController.onDrop += OnDrop;
            callbacksController.onDrag += OnDrag;
            callbacksController.onBeginDrag += OnBeginDrag;
            callbacksController.onEndDrag += OnEndDrag;
        }

        private void OnDisable()
        {
            callbacksController.onDrop -= OnDrop;
            callbacksController.onDrag -= OnDrag;
            callbacksController.onBeginDrag -= OnBeginDrag;
            callbacksController.onEndDrag -= OnEndDrag;
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

        //method is called when a cell is thrown into another cell
        private void OnDrop(InventoryCell cell, PointerEventData eventData)
        {
            if (_dragCell != null && cell != null && _dragCell != cell)
            {
                if (!cell.CanBeDropped(_dragCell))
                    return;

                bool inventoryChanged = false;

                if (eventData.button == PointerEventData.InputButton.Middle)
                {
                    if (cell.ItemDataSO == null)
                    {
                        SplitItems(cell, _dragCell);
                        inventoryChanged = true;
                    }
                    else if (cell.ItemDataSO == _dragCell.ItemDataSO)
                    {
                        MoveItemsCount(cell, _dragCell, _dragCell.ItemsCount / 2);
                        inventoryChanged = true;
                    }
                }
                else if (cell.ItemDataSO != _dragCell.ItemDataSO)
                {
                    SwapCells(cell, _dragCell);
                    inventoryChanged = true;
                }
                else if (cell.ItemDataSO == _dragCell.ItemDataSO)
                {
                    MoveItemsCount(cell, _dragCell, _dragCell.ItemsCount);
                    inventoryChanged = true;
                }

                cell.UpdateCellUI();
                _dragCell.UpdateCellUI();

                // Nếu có thay đổi, thông báo để save inventory
                if (inventoryChanged)
                {
                    // callbacksController.onInventoryChanged?.Invoke();
                }
            }
        }

        private void SplitItems(InventoryCell cell1, InventoryCell cell2)
        {
            var half = cell2.ItemsCount / 2;
            if (half > 0)
            {
                cell1.SetInventoryItem(cell2.ItemDataSO);
                cell1.ItemsCount += half;
                cell2.ItemsCount -= half;
            }
        }

        private void MoveItemsCount(InventoryCell cell1, InventoryCell cell2, int count)
        {
            if (count == 0)
                return;
            if (cell1.ItemsCount + count <= cell1.ItemDataSO.itemData.maxStack)
            {
                cell1.ItemsCount += count;
                cell2.ItemsCount -= count;
                if (cell2.ItemsCount <= 0)
                    cell2.SetInventoryItem(null);
            }
            else
            {
                if (cell1.ItemsCount < cell1.ItemDataSO.itemData.maxStack)
                {
                    cell2.ItemsCount -= (cell1.ItemDataSO.itemData.maxStack - cell1.ItemsCount);
                    cell1.ItemsCount += (cell1.ItemDataSO.itemData.maxStack - cell1.ItemsCount);
                }
            }
        }

        private void SwapCells(InventoryCell cell1, InventoryCell cell2)
        {
            var itemsSwap = (cell1.ItemDataSO, cell2.ItemDataSO);
            var itemsCountSwap = (cell1.ItemsCount, cell2.ItemsCount);

            cell1.SetInventoryItem(itemsSwap.Item2);
            cell2.SetInventoryItem(itemsSwap.Item1);

            cell1.ItemsCount = itemsCountSwap.Item2;
            cell2.ItemsCount = itemsCountSwap.Item1;
        }

        private void OnBeginDrag(InventoryCell cell, PointerEventData eventData)
        {
            if (cell.ItemDataSO == null)
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
