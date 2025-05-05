using System.Collections.Generic;
using UnityEngine;

public class InventoryDataManager : MonoBehaviour
{
    public static InventoryDataManager Instance { get; private set; }

    public string inventoryDataName = "DefaultInventory"; // Tên của ScriptableObject chứa dữ liệu mặc định
    private InventoryData inventoryData; // Dữ liệu Inventory mặc định
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(); // Lưu dữ liệu Inventory

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ dữ liệu khi chuyển Scene
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadInventory()
    {
        // Tự động tìm và nạp InventoryData từ Resources theo tên
        inventoryData = Resources.Load<InventoryData>($"InventorySO/{inventoryDataName}");

        if (inventoryData != null)
        {
            inventorySlots = new List<InventorySlot>(inventoryData.defaultInventory);
            Debug.Log($"[InventoryDataManager] Đã load InventoryData: {inventoryDataName}");
        }
        else
        {
            Debug.LogWarning($"[InventoryDataManager] Không tìm thấy InventoryData có tên {inventoryDataName} trong Resources!");
        }
    }

    public void UpdateInventory(List<InventorySlot> updatedInventory)
    {
        inventorySlots = new List<InventorySlot>(updatedInventory);
    }
}
