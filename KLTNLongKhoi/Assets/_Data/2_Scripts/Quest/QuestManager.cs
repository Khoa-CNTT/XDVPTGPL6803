using UnityEngine;
using System.Collections.Generic;
using System;

namespace KLTNLongKhoi
{
    public class QuestManager : MonoBehaviour
    {
        private List<Quest> quests = new List<Quest>();
        private SaveLoadManager saveLoadManager;
        public event Action OnQuestUpdated;
        private void Awake()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            LoadQuests();
        } 

        private void OnEnable()
        {
            // Đăng ký lắng nghe mọi sự kiện
            GameEvents.OnGameEvent += HandleGameEvent;
            saveLoadManager.OnLoaded += LoadQuestProgress;
        }

        private void OnDisable()
        {
            GameEvents.OnGameEvent -= HandleGameEvent;
            saveLoadManager.OnLoaded -= LoadQuestProgress;
        }

        private void HandleGameEvent(GameEventType eventType, object data)
        {
            switch (eventType)
            {
                case GameEventType.EnemyDefeated:
                    string enemyId = (string)data;
                    if (enemyId == "wood_boss")
                        UpdateQuestProgress("main_quest1", "Đánh bại Mộc Tinh", 1);
                    else if (enemyId == "stone_boss")
                        UpdateQuestProgress("main_quest2", "Đánh bại Sơn Tinh", 1);
                    else
                        UpdateQuestProgress("side_quest1", "Tiêu diệt quái", 1);
                    break;
                case GameEventType.ItemCrafted:
                    UpdateQuestProgress("side_quest2", "Chế tạo vật phẩm", 1);
                    break;
                case GameEventType.ItemUpgraded:
                    UpdateQuestProgress("side_quest3", "Nâng cấp vật phẩm", 1);
                    break;
                    // ...
            }
        }

        private void LoadQuests()
        {
            // Load all quests from Resources folder
            Quest[] allQuests = Resources.LoadAll<Quest>("Quests");

            foreach (var quest in allQuests)
            {
                quest.Initialize();
                quests.Add(quest);
            }
        }

        public List<Quest> GetQuests()
        {
            return quests;
        }

        public void UpdateQuestProgress(string questID, string objectiveDescription, int amount)
        {
            Debug.Log("Update quest progress: " + questID + " - " + objectiveDescription + " - " + amount);
            Quest quest = quests.Find(q => q.questID == questID); // Tìm kiếm nhiệm vụ theo ID
            if (quest != null && quest.status != QuestStatus.Completed) // Kiểm tra nếu nhiệm vụ tồn tại và chưa hoàn thành
            {
                quest.status = QuestStatus.InProgress;
                QuestObjective objective = quest.objectives.Find(o => o.objectiveDescription == objectiveDescription); // Tìm kiếm mục tiêu theo mô tả
                Debug.Log("Objective found: " + objective);
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
                    saveLoadManager.SaveQuestProgress(quests);
                }
            }
        }

        private void CompleteQuest(Quest quest)
        {
            quest.status = QuestStatus.Completed;

            // Give rewards
            GameData gameData = SaveLoadManager.Instance.GetGameData();
            gameData.player.experience += quest.experienceReward;
            gameData.player.money += quest.moneyReward;

            // Thông báo UI cập nhật
            OnQuestUpdated?.Invoke();

            SaveLoadManager.Instance.SaveData(gameData.player);
        }

        public void ResetQuests()
        {
            quests.Clear();
            LoadQuests(); // Reload all quests
        }

        public void LoadQuestProgress()
        {
            List<Quest> questsData = saveLoadManager.GetQuests();
            if (questsData.Count == 0) return;
            quests.Clear();
            quests = questsData;
        }
    }
}