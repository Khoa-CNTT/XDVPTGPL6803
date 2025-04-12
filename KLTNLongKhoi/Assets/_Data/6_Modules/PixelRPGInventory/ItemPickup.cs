using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemInventory item;
    public int quantity = 1; // Số lượng nhặt được (mặc định là 1)

    public void Pickup()
    {
        if (item != null)
        {
            Debug.Log("Nhặt: " + item.ItemName);

            bool added = InventoryManager.Instance.AddItem(item, quantity);

            if (added)
            {
                Destroy(gameObject); // Chỉ hủy object nếu item được thêm vào inventory
            }
            else
            {
                Debug.Log("Inventory đầy! Không thể nhặt " + item.ItemName);
            }
        }
        else
        {
            Debug.LogWarning("ItemPickup không có item!");
        }
    }
}
