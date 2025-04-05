using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveLoadManager saveLoadManager;
    [SerializeField] private Button saveButton;    // Nút Save trong menu
    [SerializeField] private Button loadButton;    // Nút Load trong menu
    [SerializeField] private GameObject saveLoadPanel; // Reference to the panel

    private void Start()
    {
        saveButton.onClick.AddListener(() => {
            saveLoadManager.ShowSavePanel();
        });

        loadButton.onClick.AddListener(() => {
            saveLoadManager.ShowLoadPanel();
        });

        // Đảm bảo panel ẩn khi bắt đầu
        if (saveLoadPanel != null)
        {
            saveLoadPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ấn ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSaveLoadPanel();
        }
    }

    private void ToggleSaveLoadPanel()
    {
        if (saveLoadPanel != null)
        {
            // Nếu panel đang ẩn thì hiện lên và ngược lại
            bool isActive = saveLoadPanel.activeSelf;
            saveLoadPanel.SetActive(!isActive);
            
            // Nếu đang hiện panel, refresh các slot
            if (!isActive)
            {
                saveLoadManager.ShowSavePanel();
            }
        }
    }
} 