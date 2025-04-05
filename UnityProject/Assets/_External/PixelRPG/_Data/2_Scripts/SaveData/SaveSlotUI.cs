using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private Button slotButton; // Nút để chọn slot lưu
    [SerializeField] private TextMeshProUGUI slotNumberText;  // Hiển thị số thứ tự của slot
    [SerializeField] private TextMeshProUGUI timeText;        // Hiển thị thời gian lưu trong DataContainer
    [SerializeField] private TextMeshProUGUI chapterNameText; // Hiển thị tên chương trong DataContainer
    [SerializeField] private RawImage screenshotImage;        // Hiển thị ảnh chụp màn hình của slot lưu
    [SerializeField] private GameObject noDataText;           // Hiển thị khi không có dữ liệu lưu
    [SerializeField] private GameObject dataContainer;        // Chứa các thông tin dữ liệu lưu

    private int slotIndex; // Chỉ số của slot lưu
    private Action<int> onSlotSelected; // Callback khi slot được chọn

    // Phương thức khởi tạo slot lưu
    public void Initialize(int index, Action<int> callback)
    {
        slotIndex = index; // Gán chỉ số slot
        onSlotSelected = callback; // Gán callback
        slotButton.onClick.AddListener(OnSlotClicked); // Thêm sự kiện click cho nút slot
        slotNumberText.text = $"{index + 1}.";  // Hiển thị số thứ tự của slot
        UpdateSlotUI(); // Cập nhật giao diện slot
    }

    // Phương thức cập nhật giao diện slot
    private void UpdateSlotUI()
    {
        // Đường dẫn đến file lưu
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "saves", $"save_{slotIndex}.json");
        if (System.IO.File.Exists(savePath)) // Kiểm tra nếu file lưu tồn tại
        {
            // Đọc dữ liệu từ file lưu
            string json = System.IO.File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            
            dataContainer.SetActive(true); // Hiển thị container dữ liệu
            noDataText.SetActive(false); // Ẩn thông báo không có dữ liệu
            
            // Hiển thị thời gian lưu và tên chương
            timeText.text = saveData.saveTime.ToString("MMM dd, HH:mm");
            chapterNameText.text = saveData.chapterName;

            // Kiểm tra và hiển thị ảnh chụp màn hình nếu có
            if (!string.IsNullOrEmpty(saveData.screenshotBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(saveData.screenshotBase64);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);
                screenshotImage.texture = tex;
            }
        }
        else // Nếu file lưu không tồn tại
        {
            dataContainer.SetActive(false); // Ẩn container dữ liệu
            noDataText.SetActive(true); // Hiển thị thông báo không có dữ liệu
            noDataText.GetComponent<TextMeshProUGUI>().text = "NO DATA"; // Đặt nội dung thông báo
        }
    }

    // Phương thức xử lý khi slot được chọn
    private void OnSlotClicked()
    {
        onSlotSelected?.Invoke(slotIndex); // Gọi callback với chỉ số slot
    }
}