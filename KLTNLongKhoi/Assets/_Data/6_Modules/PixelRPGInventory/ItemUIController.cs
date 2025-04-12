using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ItemInventory item;
    private int quantity;
    private int slotIndex; // Lưu vị trí slot trong inventory
    public GameObject frame; // Thêm biến lưu Frame
    private static ItemUIController selectedSlot = null; // Lưu slot đang chọn
    public void SetItem(ItemInventory newItem, int newQuantity, int index)
    {
        item = newItem;
        quantity = newQuantity;
        slotIndex = index; // Lưu lại index của slot này
    }

    public void RemoveItem()
    {
        if (item != null)
        {
            InventoryManager.Instance.RemoveItemAt(slotIndex, 1); // Truyền đúng slotIndex
        }
    }


    public void UseItem()
    {
        if (item == null)
        {
            Debug.LogWarning("Không có item");
            return;
        }

        switch (item.ScriptEffect)
        {
            case ItemInventory.ScriptType.None:
                Debug.LogWarning($"Item '{item.ItemName}' không có hiệu ứng.");
                break;

            case ItemInventory.ScriptType.ModifyStatffect:
            
                break;

            case ItemInventory.ScriptType.AffectionNpc:
                break;

            default:
                Debug.LogWarning($"Item '{item.ItemName}' chưa được gắn ScriptEffect hợp lệ.");
                break;
        }
    }

    // Sự kiện khi click chuột vào item
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // Kiểm tra có phải chuột phải không
        {
            UseItem(); // Gọi UseItem khi click chuột phải
        }
        if (selectedSlot == this) // Nếu click vào slot đang chọn thì hủy chọn
        {
            DeselectSlot();
        }
        else // Nếu click vào slot mới thì chọn slot đó
        {
            SelectSlot();
        }
    }
    private void SelectSlot()
    {
        // Chọn slot hiện tại
        if(item != null)
        {
            selectedSlot = this;
            InventoryManager.Instance.panelDescription.SetActive(true);
            ItemUIUpdater.Instance.UpdateUI(item);
        }
        
    }

    private void DeselectSlot()
    {
        InventoryManager.Instance.panelDescription.SetActive(false);
        ItemUIUpdater.Instance.ClearUI();
        selectedSlot = null;
    }

    // Khi di chuột vào item, cập nhật UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Bật Frame khi di chuột vào
        if (frame != null)
        {
            frame.SetActive(true);
        }
    }

    // Khi rời chuột khỏi item, xóa UI
    public void OnPointerExit(PointerEventData eventData)
    {
        if (frame != null)
        {
            frame.SetActive(false);
        }
    }
}
