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
    }

    [Serializable]
    public class PlayerData
    {
        public float baseHP = 5;
        public float baseSP = 5;
        public float baseMP = 5;
        public float baseStr = 5;
        public float baseCri = 5;
        public float baseInt = 5;
        public float money = 0;
        public float level = 1;
        public float experience = 0;
        public Vector3 position;
        public List<ItemData> inventory;
    }

    [Serializable]
    public class ItemData
    {
        public string id;
        public string itemName;
        public string description;
        public ItemType itemType;
        public int currentCount = 1;
        public int maxStack = 1;

        // Bonus stats
        public int physicalDamage;
        public int magicDamage;
        public int defensePoints;
        public int resistance; // sức chống cự 
        public float attackSpeed;
        public int healthRecovery;
        public int manaRecovery;
        public float criticalChance;
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

        public float mouseSensitivity = 1f;
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
}
