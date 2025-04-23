using UnityEngine;
using KLTNLongKhoi;
using TMPro;
using System.Collections;

namespace KLTNLongKhoi
{
    public class WritePlayerName : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private GameObject nameInputPanel;
        [SerializeField] GameObject menuPanel;
        [SerializeField] GameObject areYouSurePanel; // Panel xác nhận xóa tên

        private void Start()
        {
            // Kiểm tra tên trong data
            if (string.IsNullOrWhiteSpace(DataManager.Instance.GameData.player.name))
            {
                nameInputPanel.SetActive(true); // Hiện panel nếu chưa có tên
                menuPanel.SetActive(false); // Tắt menu panel khi nhập tên 
            }
            else
            {
                nameInputPanel.SetActive(false); 
                areYouSurePanel.SetActive(false);
            }
        }

        private void Update() {
            // Khi người chơi nhấn enter hoặc gọi hàm OpenAreYouSurePanel() thì hiện panel xác nhận xóa tên
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OpenAreYouSurePanel();
            }
        }

        public void OpenAreYouSurePanel()
        {
            areYouSurePanel.SetActive(true);
        }

        // Hàm này sẽ được gọi từ button "Confirm" hoặc "Accept"
        public void ConfirmAndSaveName()
        {
            if (nameInputField == null || string.IsNullOrWhiteSpace(nameInputField.text))
            {
                Debug.LogWarning("Please enter a valid name!");
                return;
            }

            if (DataManager.Instance != null)
            {
                menuPanel.SetActive(true);
                DataManager.Instance.GameData.player.name = nameInputField.text;
                DataManager.Instance.ArchiveGameData();
            }
            else Debug.LogWarning("DataManager not found!");
        }
    }

}