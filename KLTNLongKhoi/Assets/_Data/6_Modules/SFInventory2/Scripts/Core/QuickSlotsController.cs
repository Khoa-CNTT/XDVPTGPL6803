using UnityEngine;

namespace Parity.SFInventory2.Core
{
    //just an example of quick slots for your game
    public class QuickSlotsController : ContainerBase
    {

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
