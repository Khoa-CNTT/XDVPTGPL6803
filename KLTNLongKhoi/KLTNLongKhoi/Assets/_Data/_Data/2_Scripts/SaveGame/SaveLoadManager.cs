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

        public event Action OnLoaded;

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
            return dataManager.GameData.player.IsNewGameplay;
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
            dataManager.GameData.questProgress = new List<QuestProgressData>();
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
            else if (typeof(T) == typeof(List<Quest>))
            {
                return (T)(object)GetQuests();
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
            else if (data is List<MonsterData> monstersData)
            {
                dataManager.GameData.monsters = monstersData;
            }
            else if (data is List<Quest> questsData)
            {
                SaveQuestProgress(questsData);
            }
            else
            {
                Debug.LogError("Type not supported for saving: " + typeof(D));
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

        public List<Quest> GetQuests()
        {
            return dataManager.GameData.questProgress.Select(p =>
            {
                Quest quest = null;
                // Tìm tất cả Quest trong thư mục Resources/Quests
                Quest[] allQuests = Resources.LoadAll<Quest>("Quests");
                // Tìm Quest có questID trùng khớp
                quest = allQuests.FirstOrDefault(q => q.questID == p.questID);

                if (quest == null)
                {
                    Debug.LogWarning($"Không tìm thấy Quest với ID: {p.questID}");
                    return null;
                }

                quest.status = p.status;
                foreach (var objectiveProgress in p.objectives)
                {
                    QuestObjective objective = quest.objectives.Find(o =>
                        o.objectiveDescription == objectiveProgress.objectiveDescription);
                    if (objective != null)
                    {
                        objective.currentAmount = objectiveProgress.currentAmount;
                    }
                }
                return quest;
            }).ToList();
        }
    }
}
