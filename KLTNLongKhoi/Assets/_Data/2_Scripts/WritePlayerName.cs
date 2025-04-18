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
        private void Start()
        {
            // Kiểm tra tên trong data
            if (string.IsNullOrWhiteSpace(DataManager.Instance.GameData.player.name))
            {
                nameInputPanel.SetActive(true); // Hiện panel nếu chưa có tên

            }
            else
            {
                nameInputPanel.SetActive(false);
            }
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
                DataManager.Instance.GameData.player.name = nameInputField.text;
                DataManager.Instance.ArchiveGameData();
            }
        }
    }

}