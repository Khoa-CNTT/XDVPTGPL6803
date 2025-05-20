using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class QuestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Transform questsContainer;
        [SerializeField] private Button btnOpenPanel;
        [SerializeField] private Button btnClosePanel;
        [SerializeField] private Button btnShowMainQuest;
        [SerializeField] private Button btnShowSideQuest;
        [SerializeField] private OnTriggerThis onTriggerThis;
        private QuestManager questManager;
        private PauseManager pauseManager;
        private StarterAssetsInputs starterAssetsInputs;

        private bool isPanelOpen = false;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            questManager = FindFirstObjectByType<QuestManager>();
            onTriggerThis = GetComponentInChildren<OnTriggerThis>();
        }

        private void OnEnable()
        {
            btnOpenPanel?.onClick.AddListener(OpenQuestPanel);
            btnClosePanel?.onClick.AddListener(CloseQuestPanel);
            btnShowMainQuest.onClick.AddListener(ShowMainQuest);
            btnShowSideQuest.onClick.AddListener(ShowSideQuest);
            if (starterAssetsInputs) starterAssetsInputs.QuestPanel += OnToggleQuestPanel;
        }

        private void OnDisable()
        {
            btnOpenPanel?.onClick.RemoveListener(OpenQuestPanel);
            btnClosePanel?.onClick.RemoveListener(CloseQuestPanel);
            btnShowMainQuest.onClick.RemoveListener(ShowMainQuest);
            btnShowSideQuest.onClick.RemoveListener(ShowSideQuest);
            if (starterAssetsInputs) starterAssetsInputs.QuestPanel -= OnToggleQuestPanel;
        }

        private void OnToggleQuestPanel()
        {
            isPanelOpen = !isPanelOpen;

            if (isPanelOpen)
            {
                OpenQuestPanel();
            }
            else
            {
                CloseQuestPanel();
            }
        }
 
        public void OpenQuestPanel()
        {
            isPanelOpen = true;
            ShowMainQuest();
            pauseManager?.PauseGame();
            onTriggerThis.ActiveObjects();
        }

        public void CloseQuestPanel()
        {
            isPanelOpen = false;
            pauseManager?.ResumeGame();
            onTriggerThis.UnActiveObjects();
        }

        private void ShowMainQuest()
        { 
            foreach (Transform child in questsContainer)
            {
                Destroy(child.gameObject);
            }
 
            foreach (var quest in GetQuestsByType(QuestType.Main))
            {
                CreateQuestUI(quest);
            }
        }

        private void ShowSideQuest()
        {
            foreach (Transform child in questsContainer)
            {
                Destroy(child.gameObject);
            }

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

        private List<Quest> GetQuests() => questManager.GetQuests();

        private List<Quest> GetQuestsByType(QuestType questType)
        {
            return questManager.GetQuests().FindAll(q => q.questType == questType);
        }


    }
}