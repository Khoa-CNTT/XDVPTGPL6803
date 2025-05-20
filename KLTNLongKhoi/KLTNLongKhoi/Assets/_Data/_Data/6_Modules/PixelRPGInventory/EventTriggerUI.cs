using UnityEngine;
using UnityEngine.UI;

public class EventTriggerUI : MonoBehaviour
{
    public GameObject inventoryPanel; // Kéo thả trực tiếp trong Inspector
    public GameObject statPanel; // Kéo thả trực tiếp trong Inspector
    public Toggle toggleRemoveItem; // Kéo thả Toggle trong Inspector
    public GameObject hidePanel;
    private int activePanelCount = 0; // Đếm số panel đang mở

    void Start()
    {
        // Kiểm tra nếu panel mở sẵn từ đầu, tránh bị pause sai
        if (inventoryPanel.activeSelf) activePanelCount++;
        if (statPanel.activeSelf) activePanelCount++;

        UpdateGamePauseState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Nhấn 'I' để bật/tắt Inventory
        {
            ToggleUI(inventoryPanel);
            InventoryManager.Instance.UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.O)) // Nhấn 'O' để bật/tắt Stat
        {
            ToggleUI(statPanel);
        }
    }

    public void ToggleUI(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = !panel.activeSelf;
            panel.SetActive(isActive);

            if (isActive) activePanelCount++;
            else activePanelCount--;

            UpdateGamePauseState();

            // Nếu Inventory bị tắt, reset toggle về false
            if (panel == inventoryPanel && !isActive && toggleRemoveItem != null)
            {
                toggleRemoveItem.isOn = false;
            }

            // Kiểm tra và cập nhật hidePanel
            UpdateHidePanelState();
        }
    }

    private void UpdateHidePanelState()
    {
        if (activePanelCount > 0)
        {
            // Chỉ bật hidePanel nếu nó chưa được bật
            if (!hidePanel.activeSelf)
            {
                hidePanel.SetActive(true);
            }
        }
        else
        {
            hidePanel.SetActive(false);
        }
    }
    private void UpdateGamePauseState()
    {
        // Nếu có ít nhất 1 panel mở -> pause game, không thì resume
        Time.timeScale = (activePanelCount > 0) ? 0 : 1;
    }
}
