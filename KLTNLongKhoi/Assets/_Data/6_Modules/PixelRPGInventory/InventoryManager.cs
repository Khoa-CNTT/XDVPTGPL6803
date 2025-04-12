using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton

    [SerializeField] private int slotCount = 9; // Số lượng slot trong inventory
    [SerializeField] private GameObject slotPrefab; // Prefab của một slot
    [SerializeField] private Transform content; // Content chứa các slot trong Scroll View

    [SerializeField] private GameObject[] slots; // Mảng chứa các slot UI
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // Danh sách slot trong inventory
    [SerializeField] private Toggle removeToggle; // Toggle để bật/tắt RemoveButton
    public GameObject panelDescription; // Gán trong Inspector

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        InitializeSlots();
        UpdateUI();
    }

    private void Start()
    {
        if (InventoryDataManager.Instance != null)
        {
            inventorySlots = new List<InventorySlot>(InventoryDataManager.Instance.inventorySlots);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Dùng inventory mặc định của InventoryManager");
        }
    }

    private void InitializeSlots()
    {
        slots = new GameObject[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, content);
            slots[i] = slot;
            slot.name = "Slot_" + i;
        }
    }
    private void SaveInventory()
    {
        if (InventoryDataManager.Instance != null)
        {
            InventoryDataManager.Instance.UpdateInventory(inventorySlots);
        }
    }

    public bool AddItem(ItemInventory newItem, int quantity)
    {
        // Trường hợp 1: Cộng dồn vào slot đã có item cùng loại
        foreach (var slot in inventorySlots)
        {
            if (slot.item != null && slot.item.Id == newItem.Id && slot.quantity < newItem.MaxStack)
            {
                int addable = Mathf.Min(quantity, newItem.MaxStack - slot.quantity);
                slot.quantity += addable;
                quantity -= addable;

                if (quantity <= 0)
                {
                    SaveInventory();
                    UpdateUI();
                    return true;
                }
            }
        }

        // Trường hợp 2: Tìm slot trống để điền vào
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == null) // Slot trống
            {
                inventorySlots[i] = new InventorySlot(newItem, Mathf.Min(quantity, newItem.MaxStack));
                SaveInventory();
                UpdateUI();
                return true;
            }
        }

        // Trường hợp 3: Nếu không có slot trống, thêm vào cuối nếu còn chỗ
        if (inventorySlots.Count < slotCount)
        {
            inventorySlots.Add(new InventorySlot(newItem, Mathf.Min(quantity, newItem.MaxStack)));
            SaveInventory();
            UpdateUI();
            return true;
        }

        Debug.Log("Inventory đầy!");
        return false;
    }


    public void RemoveItemAt(int slotIndex, int quantity = 1)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
        {
            int removeAmount = Mathf.Min(quantity, inventorySlots[slotIndex].quantity);
            inventorySlots[slotIndex].quantity -= removeAmount;

            if (inventorySlots[slotIndex].quantity <= 0)
            {
                inventorySlots.RemoveAt(slotIndex);
            }
            SaveInventory();
            UpdateUI();
        }
    }


    public void UpdateUI()
    {
        // Xóa toàn bộ child của content trước khi cập nhật UI
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        InitializeSlots(); // Tạo lại các slot mới

        for (int i = 0; i < slots.Length; i++)
        {
            Transform imageItem = slots[i].transform.Find("ImageItem");
            Image img = imageItem?.GetComponent<Image>();
            TMP_Text quantityText = slots[i].transform.Find("QuantityText")?.GetComponent<TMP_Text>();
            ItemUIController itemUI = slots[i].GetComponent<ItemUIController>();

            if (img != null && quantityText != null)
            {
                if (i < inventorySlots.Count && inventorySlots[i].item != null) // Slot có item
                {
                    img.sprite = inventorySlots[i].item.Image;
                    img.enabled = true; // Hiển thị ảnh item
                    itemUI.SetItem(inventorySlots[i].item, inventorySlots[i].quantity, i); // Truyền slot index

                    quantityText.text = inventorySlots[i].quantity > 1 ? inventorySlots[i].quantity.ToString() : "";
                    quantityText.enabled = inventorySlots[i].quantity > 1;

                }
                else // Slot trống
                {
                    img.sprite = null;
                    img.enabled = false; // Ẩn ảnh nhưng giữ slot
                    itemUI.SetItem(null, 0, i); // Xóa item trong slot trống
                    quantityText.text = "";
                    quantityText.enabled = false; // Ẩn số lượng
                }
            }
        }
        ToggleRemoveButton();
    }

    public void ToggleRemoveButton()
    {
        bool isOn = removeToggle.isOn; // Kiểm tra trạng thái Toggle

        for (int i = 0; i < slots.Length; i++)
        {
            Transform removeButton = slots[i].transform.Find("RemoveButton");
            if (removeButton != null)
            {
                // Chỉ hiển thị RemoveButton nếu toggle bật và slot có item
                bool hasItem = i < inventorySlots.Count && inventorySlots[i].item != null;
                removeButton.gameObject.SetActive(isOn && hasItem);
            }
        }
    }


}

//public bool AddItem(ItemInventory newItem, int quantity)
//{
//    foreach (var slot in inventorySlots)
//    {
//        if (slot.item.Id == newItem.Id && slot.quantity < newItem.MaxStack)
//        {
//            int addable = Mathf.Min(quantity, newItem.MaxStack - slot.quantity);
//            slot.quantity += addable;
//            quantity -= addable;

//            if (quantity <= 0)
//            {
//                UpdateUI();
//                return true; // Thêm thành công
//            }
//        }
//    }

//    if (inventorySlots.Count < slotCount)
//    {
//        InventorySlot newSlot = new InventorySlot(newItem, Mathf.Min(quantity, newItem.MaxStack));
//        inventorySlots.Add(newSlot);
//        UpdateUI();
//        return true; // Thêm thành công
//    }

//    Debug.Log("Inventory đầy!");
//    return false; // Không thêm được
//}

//public void RemoveItemAt(int slotIndex, int quantity = 1)
//{
//    if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
//    {
//        int removeAmount = Mathf.Min(quantity, inventorySlots[slotIndex].quantity);
//        inventorySlots[slotIndex].quantity -= removeAmount;

//        if (inventorySlots[slotIndex].quantity <= 0)
//        {
//            inventorySlots.RemoveAt(slotIndex);
//        }

//        UpdateUI();
//    }
//}

//public void RemoveItem(ItemInventory item, int quantity = 1)
//{
//    for (int i = 0; i < inventorySlots.Count; i++)
//    {
//        if (inventorySlots[i].item.Id == item.Id)
//        {
//            int removeAmount = Mathf.Min(quantity, inventorySlots[i].quantity);
//            inventorySlots[i].quantity -= removeAmount;

//            if (inventorySlots[i].quantity <= 0)
//            {
//                inventorySlots.RemoveAt(i);
//            }

//            UpdateUI();
//            return;
//        }
//    }
//}

//public void RemoveItemAt(int slotIndex, int quantity = 1)
//{
//    if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
//    {
//        int removeAmount = Mathf.Min(quantity, inventorySlots[slotIndex].quantity);
//        inventorySlots[slotIndex].quantity -= removeAmount;

//        if (inventorySlots[slotIndex].quantity <= 0)
//        {
//            inventorySlots[slotIndex] = new InventorySlot(null, 0); // Slot vẫn tồn tại nhưng trống
//        }

//        UpdateUI();
//    }
//}