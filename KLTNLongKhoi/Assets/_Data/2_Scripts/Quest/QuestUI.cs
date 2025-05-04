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
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Transform questsContainer;
        [SerializeField] private Button btnOpenPanel;
        [SerializeField] private Button btnClosePanel;
        [SerializeField] private Button btnShowMainQuest;
        [SerializeField] private Button btnShowSideQuest;

        private bool isPanelOpen = false;

        private void OnEnable()
        {
            btnOpenPanel.onClick.AddListener(OpenQuestPanel);
            btnClosePanel.onClick.AddListener(CloseQuestPanel);
            btnShowMainQuest.onClick.AddListener(ShowMainQuest);
            btnShowSideQuest.onClick.AddListener(ShowSideQuest);
        }

        private void OnDisable()
        {
            btnOpenPanel.onClick.RemoveListener(OpenQuestPanel);
            btnClosePanel.onClick.RemoveListener(CloseQuestPanel);
            btnShowMainQuest.onClick.RemoveListener(ShowMainQuest);
            btnShowSideQuest.onClick.RemoveListener(ShowSideQuest);
        }

        public void OpenQuestPanel()
        {
            isPanelOpen = true;
            questPanel.SetActive(isPanelOpen);
            ShowMainQuest();
        }

        public void CloseQuestPanel()
        {
            isPanelOpen = false;
            questPanel.SetActive(isPanelOpen);
        }

        private void ShowMainQuest()
        {
            // Xóa các quest UI cũ
            foreach (Transform child in questsContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo UI cho các nhiệm vụ chính
            foreach (var quest in GetQuestsByType(QuestType.Main))
            {
                CreateQuestUI(quest);
            }
        }

        private void ShowSideQuest()
        {
            // Xóa các quest UI cũ
            foreach (Transform child in questsContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo UI cho các nhiệm vụ支线任务
            foreach (var quest in GetQuestsByType(QuestType.Side))
            {
                CreateQuestUI(quest);
            }
        }

        private void CreateQuestUI(Quest quest)
        {
            GameObject questUI = Instantiate(questPrefab, questsContainer);
            questUI.GetComponent<QuestUIElement>().Initialize(quest);
        }

        private List<Quest> GetQuests() => QuestManager.Instance.GetQuests();

        private List<Quest> GetQuestsByType(QuestType questType)
        {
            return QuestManager.Instance.GetQuests().FindAll(q => q.questType == questType);
        }


    }
}