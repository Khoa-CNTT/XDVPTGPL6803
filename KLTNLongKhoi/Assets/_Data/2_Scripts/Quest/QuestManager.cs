using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KLTNLongKhoi
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [SerializeField] private List<QuestData> availableQuests;
        private Dictionary<string, QuestStatus> questStatuses = new Dictionary<string, QuestStatus>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeQuests();
        }

        private void InitializeQuests()
        {
            foreach (var quest in availableQuests)
            {
                questStatuses[quest.questID] = new QuestStatus(quest.questID);
            }
        }

        public void ActivateQuest(string questID)
        {
            if (!questStatuses.ContainsKey(questID)) return;

            QuestData quest = availableQuests.Find(q => q.questID == questID);
            if (quest == null) return;

            // Kiểm tra điều kiện tiên quyết
            if (quest.prerequisiteQuests != null && quest.prerequisiteQuests.Length > 0)
            {
                foreach (var prereq in quest.prerequisiteQuests)
                {
                    if (!IsQuestCompleted(prereq.questID)) return;
                }
            }

            questStatuses[questID].isActive = true;
        }

        public void UpdateQuestProgress(string questID, int amount)
        {
            if (!questStatuses.ContainsKey(questID) || !questStatuses[questID].isActive) return;

            QuestData quest = availableQuests.Find(q => q.questID == questID);
            if (quest == null) return;

            QuestStatus status = questStatuses[questID];
            status.currentAmount += amount;

            if (status.currentAmount >= quest.requiredAmount)
            {
                CompleteQuest(questID);
            }
        }

        public void CompleteQuest(string questID)
        {
            if (!questStatuses.ContainsKey(questID)) return;

            QuestStatus status = questStatuses[questID];
            status.isCompleted = true;
            status.isActive = false;

            // Thực hiện phần thưởng ở đây
            QuestData quest = availableQuests.Find(q => q.questID == questID);
            if (quest != null)
            {
                // Thêm gold
                // GameManager.Instance.AddGold(quest.goldReward);

                // Thêm items
                if (quest.itemRewards != null)
                {
                    foreach (var item in quest.itemRewards)
                    {
                        // Inventory.Instance.AddItem(item);
                    }
                }
            }
        }

        public bool IsQuestActive(string questID)
        {
            return questStatuses.ContainsKey(questID) && questStatuses[questID].isActive;
        }

        public bool IsQuestCompleted(string questID)
        {
            return questStatuses.ContainsKey(questID) && questStatuses[questID].isCompleted;
        }

        public QuestStatus GetQuestStatus(string questID)
        {
            return questStatuses.ContainsKey(questID) ? questStatuses[questID] : null;
        }
    }
}