using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIUpdater : MonoBehaviour
{
    public static ItemUIUpdater Instance;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemDescriptionText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Ban đầu để rỗng
        ClearUI();
    }

    public void UpdateUI(ItemInventory item)
    {
        if (item == null) return;

        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.Describe;
        itemImage.sprite = item.Image;
        itemImage.enabled = item.Image != null; // Ẩn nếu không có hình ảnh
    }

    public void ClearUI()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
}
