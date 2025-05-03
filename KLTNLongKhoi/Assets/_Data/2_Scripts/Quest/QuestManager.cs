using UnityEngine;
using System.Collections.Generic;
using System;

namespace KLTNLongKhoi
{
    public class QuestManager : Singleton<QuestManager>
    {
        private List<Quest> activeQuests = new List<Quest>();
        private List<Quest> completedQuests = new List<Quest>();
        public event Action OnQuestUpdated;
        protected override void Awake()
        {
            base.Awake();
            LoadQuests();
        }



        private void LoadQuests()
        {
            // Load all quests from Resources folder
            Quest[] allQuests = Resources.LoadAll<Quest>("Quests");

            foreach (var quest in allQuests)
            {
                quest.Initialize();
                activeQuests.Add(quest);
            }
        }

        public List<Quest> GetActiveQuests()
        {
            return activeQuests;
        }

        public List<Quest> GetCompletedQuests()
        {
            return completedQuests;
        }

        public void UpdateQuestProgress(string questID, string objectiveDescription, int amount)
        {
            Quest quest = activeQuests.Find(q => q.questID == questID);
            if (quest != null && quest.status != QuestStatus.Completed)
            {
                quest.status = QuestStatus.InProgress;
                QuestObjective objective = quest.objectives.Find(o => o.objectiveDescription == objectiveDescription);
                if (objective != null)
                {
                    objective.currentAmount += amount;
                    if (quest.CheckCompletion())
                    {
                        CompleteQuest(quest);
                    }
                    else
                    {
                        // Thông báo UI cập nhật ngay cả khi chưa hoàn thành
                        OnQuestUpdated?.Invoke();
                    }
                }
            }
        }

        private void CompleteQuest(Quest quest)
        {
            quest.status = QuestStatus.Completed;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            // Give rewards
            GameData gameData = SaveLoadManager.Instance.GetGameData();
            gameData.player.experience += quest.experienceReward;
            gameData.player.money += quest.moneyReward;

            // Thông báo UI cập nhật
            OnQuestUpdated?.Invoke();

            SaveLoadManager.Instance.SaveData(gameData.player);
        }
    }
}