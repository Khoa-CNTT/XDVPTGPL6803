using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KLTNLongKhoi
{
    [Serializable]
    public class GameData
    {
        // User Data
        public GameSettingsData gameSettings = new GameSettingsData();

        // GamePlay data
        public PlayerData player = new PlayerData();
        public List<ItemData> worldItems = new List<ItemData>();
        public List<MonsterData> monsters = new List<MonsterData>();
        public List<QuestProgressData> questProgress = new List<QuestProgressData>();
    }

    [Serializable]
    public class PlayerData
    {
        public float baseHP = 500;
        public float baseSP = 50;
        public float baseMP = 50;
        public float baseStr = 200;
        public float baseCri = 100;
        public float baseInt = 200;
        public float money = 500;
        public float level = 1;
        public float experience = 0;
        public Vector3 position;
        public int levelSkillC = 1;
        public int levelSkillE = 1;
        public int levelSkillQ = 1;
        public bool IsNewGameplay = true;
        public const int maxInventorySize = 19;
        public List<ItemData> inventory;
    }

    [Serializable]
    public class ItemData
    {
        public string id;
        public string name;
        [TextArea(5, 10)]
        public string description;
        public ItemType itemType;
        public int itemCount = 1; // dùng để lưu số lượng item trong túi chứ không vì mục đích nào khác
        public int maxStack = 1;
        public int price;
        public int cellIndex; // vị trí trong túi

        // Bonus stats
        public int physicalDamage;
        public int magicDamage;
        public int defensePoints;
        public int resistance; // sức chống cự 
        public float attackSpeed;
        public int healthRecovery;
        public int manaRecovery;
        public float criticalChance;

        // tỷ lệ 
        public float successRate;
    }

    [Serializable]
    public class GameSettingsData
    {
        // Audio Settings
        public float masterVolume = 1f;
        public float musicVolume = 1f;
        public float sfxVolume = 1f;

        // Graphics Settings
        public int qualityLevel = 1; // 0: Thấp, 1: Trung bình, 2: Cao
        public string resolution = "1920x1080";
        public int targetFrameRate = 60;
        public float brightness = 1f;
    }

    [Serializable]
    public class MonsterData
    {
        public string id;
        public string name;
        public string description;
        public int level;
        public int health;
        public bool isDefeated;
        public Vector3 position;
    }

    [Serializable]
    public enum ItemType
    {
        None,
        Weapon,
        Armor,
        Tool,
        Resource,
        Consumable,
        Quest
    }

    [Serializable]
    public class QuestProgressData
    {
        public string questID;
        public QuestStatus status;
        public List<ObjectiveProgressData> objectives;
    }

    [Serializable]
    public class ObjectiveProgressData
    {
        public string objectiveDescription;
        public int currentAmount;
    }
}
