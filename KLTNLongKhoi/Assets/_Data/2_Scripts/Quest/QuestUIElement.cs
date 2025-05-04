using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KLTNLongKhoi
{
    public class QuestUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questNameText;
        [SerializeField] private TextMeshProUGUI questDescriptionText;
        [SerializeField] private TextMeshProUGUI questStatusText;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button completeButton;

        private Quest quest;

        public void Initialize(Quest quest)
        {
            this.quest = quest;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (questNameText != null) questNameText.text = quest.questName;
            if (questDescriptionText != null) questDescriptionText.text = quest.description;
            if (questStatusText != null) questStatusText.text = quest.status.ToString();
            if (acceptButton != null) acceptButton.gameObject.SetActive(quest.status == QuestStatus.NotStarted);
            if (completeButton != null) completeButton.gameObject.SetActive(quest.status == QuestStatus.InProgress);
        }

        public void OnAcceptButtonClicked()
        { 
            UpdateUI();
        }

        public void OnCompleteButtonClicked()
        { 
            UpdateUI();
        }
    }
}

