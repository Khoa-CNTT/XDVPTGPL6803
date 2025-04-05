using UnityEngine;

namespace Parity.SFInventory2.Core
{
    // một ví dụ về nhanh chóng cho game của bạn
    public class QuickSlotsController : ContainerBase
    {

        void Update()
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < Input.inputString.Length; i++)
                {
                    // kiểm tra xem nút nhấn có phải là số hay không
                    if (char.IsDigit(Input.inputString[i]))
                    {
                        // chuyển đổi chuỗi thành số
                        if (int.TryParse(Input.inputString[i].ToString(), out var num))
                        {
                            num -= 1;
                            if (num >= 0 && num < inventoryCells.Count)
                            {
                                // kiểm tra xem ô có chứa đồ vật hay không
                                if (inventoryCells[num].Item != null)
                                {
                                    Debug.Log("Item Selected: " + inventoryCells[num].Item.itemName);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
