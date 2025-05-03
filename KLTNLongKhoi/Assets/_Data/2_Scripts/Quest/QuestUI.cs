using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace KLTNLongKhoi
{
    public class QuestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject questPanel;
        [SerializeField] private Transform activeQuestsContainer;
        [SerializeField] private Transform completedQuestsContainer;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Button toggleButton;
        [SerializeField] private TMP_Text toggleButtonText; 

        private bool isPanelOpen = false;

        private void Start()
        {
            toggleButton.onClick.AddListener(ToggleQuestPanel);
            UpdateQuestUI();

            // Đăng ký sự kiện khi nhiệm vụ được cập nhật
            QuestManager.Instance.OnQuestUpdated += UpdateQuestUI;
        }

        private void OnDestroy()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestUpdated -= UpdateQuestUI;
            }
        }

        public void ToggleQuestPanel()
        {
            isPanelOpen = !isPanelOpen;
            questPanel.SetActive(isPanelOpen);
            toggleButtonText.text = isPanelOpen ? "Đóng Nhiệm Vụ" : "Mở Nhiệm Vụ";

            if (isPanelOpen)
            {
                UpdateQuestUI();
            }
        }

        public void UpdateQuestUI()
        {
            // Xóa các quest UI cũ
            foreach (Transform child in activeQuestsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in completedQuestsContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo UI cho các nhiệm vụ đang thực hiện
            foreach (var quest in QuestManager.Instance.GetActiveQuests())
            {
                // CreateQuestUI(quest, activeQuestsContainer);
            }

            // Tạo UI cho các nhiệm vụ đã hoàn thành
            foreach (var quest in QuestManager.Instance.GetCompletedQuests())
            {
                // CreateQuestUI(quest, completedQuestsContainer);
            }
        }


    }
}