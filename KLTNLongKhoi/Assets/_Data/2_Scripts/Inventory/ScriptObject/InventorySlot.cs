using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemInventory item;
    public int quantity;

    public InventorySlot(ItemInventory newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
}
