using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    int selectedSlotIndex;

    Inventory inventory;

    public static InventoryController PlayerInstance { get; private set; }

    public Slot GetSelectedSlot()
    {
        return inventory.GetInventory()[selectedSlotIndex];
    }
    void Start()
    {
        PlayerInstance = this;
        inventory = Inventory.PlayerInstance;
        selectedSlotIndex = 0;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (inventory.IndexIsInRange(selectedSlotIndex - 1))
            {
                selectedSlotIndex -= 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (inventory.IndexIsInRange(selectedSlotIndex + 1))
            {
                selectedSlotIndex += 1;
            }
        }
    }
}
