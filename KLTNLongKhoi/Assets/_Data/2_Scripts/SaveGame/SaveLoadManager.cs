using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private DataManager dataManager;

        public UnityEvent OnLoaded;

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad(true);
            Init();
        }

        private void Start()
        {
            OnLoaded?.Invoke();
        }

        public void Init()
        {
            dataManager = FindFirstObjectByType<DataManager>();
        }

        // kiểm tra xem đây có phải là gameplay mới hay không
        public bool IsNewGameplay()
        {
            return dataManager.GameData.player.position == Vector3.zero;
        }

        public void ResetGameSettings()
        {
            dataManager.GameData.gameSettings = new GameSettingsData();
            dataManager.ArchiveGameData();
        }

        // Method để reset toàn bộ game state
        [ContextMenu("Reset Game Data")]
        public void ResetGameData()
        {
            Init();
            dataManager.GameData.player = new PlayerData();
            dataManager.GameData.worldItems = new List<ItemData>();
            dataManager.GameData.monsters = new List<MonsterData>();
            dataManager.ArchiveGameData();
        }

        public GameData GetGameData()
        {
            return dataManager.GameData;
        }

        public T LoadData<T>()
        {
            if (typeof(T) == typeof(PlayerData))
            {
                return (T)(object)dataManager.GameData.player;
            }
            else if (typeof(T) == typeof(GameSettingsData))
            {
                return (T)(object)dataManager.GameData.gameSettings;
            }
            else if (typeof(T) == typeof(List<ItemData>))
            {
                return (T)(object)dataManager.GameData.worldItems;
            }
            else if (typeof(T) == typeof(List<MonsterData>))
            {
                return (T)(object)dataManager.GameData.monsters;
            }
            else if (typeof(T) == typeof(List<QuestProgressData>))
            {
                return (T)(object)dataManager.GameData.questProgress;
            }
            else
            {
                Debug.LogError("Type not supported for loading: " + typeof(T));
                return default;
            }
        }

        public void SaveData<D>(D data)
        {
            if (data is PlayerData playerData)
            {
                dataManager.GameData.player = playerData;
            }
            else if (data is GameSettingsData gameSettingsData)
            {
                dataManager.GameData.gameSettings = gameSettingsData;
            }
            else if (data is ItemData itemData)
            {
                var existingItem = dataManager.GameData.worldItems.FirstOrDefault(i => i.id == itemData.id);
                if (existingItem != null)
                {
                    dataManager.GameData.worldItems.Remove(existingItem);
                }
                dataManager.GameData.worldItems.Add(itemData);
            }
            else if (data is List<ItemData> worldItemsData)
            {
                dataManager.GameData.worldItems = worldItemsData;
            }
            else if (data is MonsterData monsterData)
            {
                var existingMonster = dataManager.GameData.monsters.FirstOrDefault(m => m.id == monsterData.id);
                if (existingMonster != null)
                {
                    dataManager.GameData.monsters.Remove(existingMonster);
                }
                dataManager.GameData.monsters.Add(monsterData);
            }

            dataManager.ArchiveGameData();
        }

        public void SaveQuestProgress(List<Quest> quests)
        {
            List<QuestProgressData> progressData = new List<QuestProgressData>();

            foreach (var quest in quests)
            {
                QuestProgressData data = new QuestProgressData
                {
                    questID = quest.questID,
                    status = quest.status,
                    objectives = new List<ObjectiveProgressData>()
                };

                foreach (var objective in quest.objectives)
                {
                    data.objectives.Add(new ObjectiveProgressData
                    {
                        objectiveDescription = objective.objectiveDescription,
                        currentAmount = objective.currentAmount
                    });
                }

                progressData.Add(data);
            }

            dataManager.GameData.questProgress = progressData;
            dataManager.ArchiveGameData();
        }

        public void LoadQuestProgress(List<Quest> quests)
        {
            foreach (var progress in dataManager.GameData.questProgress)
            {
                Quest quest = quests.Find(q => q.questID == progress.questID);
                if (quest != null)
                {
                    quest.status = progress.status;

                    foreach (var objectiveProgress in progress.objectives)
                    {
                        QuestObjective objective = quest.objectives.Find(o =>
                            o.objectiveDescription == objectiveProgress.objectiveDescription);
                        if (objective != null)
                        {
                            objective.currentAmount = objectiveProgress.currentAmount;
                        }
                    }
                }
            }
        }
    }
}
