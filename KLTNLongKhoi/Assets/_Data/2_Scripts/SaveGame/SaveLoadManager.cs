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
    }
}
