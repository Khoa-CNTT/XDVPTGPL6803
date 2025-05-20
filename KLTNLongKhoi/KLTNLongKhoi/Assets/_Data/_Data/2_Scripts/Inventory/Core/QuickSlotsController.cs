using UnityEngine;

namespace KLTNLongKhoi
{
    //just an example of quick slots for your game
    public class QuickSlotsController : ContainerBase
    {
        [SerializeField] InventoryCell inventoryCellSelected;
        [SerializeField] InventoryController inventoryController;

        public InventoryCell InventoryCellSelected { get => inventoryCellSelected; set => inventoryCellSelected = value; }

        protected override void Awake()
        {
            base.Awake();
            inventoryController = FindFirstObjectByType<InventoryController>();
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < Input.inputString.Length; i++)
                {
                    if (char.IsDigit(Input.inputString[i]))
                    {
                        if (int.TryParse(Input.inputString[i].ToString(), out var num))
                        {
                            num -= 1;
                            if (num >= 0 && num < inventoryCells.Count)
                            {
                                if (InventoryCellSelected != null)
                                {
                                    InventoryCellSelected.Highlight.enabled = false;
                                }
                                if (inventoryCells[num].ItemDataSO != null)
                                {
                                    Debug.Log("Item Selected: " + inventoryCells[num].ItemDataSO.ItemData.name);
                                    InventoryCellSelected = inventoryCells[num];
                                    InventoryCellSelected.Highlight.enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Tiêu thụ item đã được chọn
        public void UseItem()
        {
            if (inventoryCellSelected != null)
            {
                if (inventoryCellSelected.ItemDataSO != null)
                {
                    if (inventoryCellSelected.ItemsCount > 0)
                    {
                        inventoryCellSelected.ItemsCount--;
                        inventoryCellSelected.UpdateCellUI();

                        // nếu số lượng item trong cell = 0 thì xóa item đó khỏi cell
                        if (inventoryCellSelected.ItemsCount == 0)
                        {
                            inventoryCellSelected.SetInventoryItem(null);
                            inventoryCellSelected.UpdateCellUI();
                        }
                        inventoryController.SaveThisInventory();
                    }
                }
            }
        }
    }
}
