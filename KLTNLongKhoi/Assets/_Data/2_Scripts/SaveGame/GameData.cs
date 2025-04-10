using System;
using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    [Serializable]
    public class GameData
    {
        public PlayerData player = new PlayerData();
        public WorldState worldState = new WorldState();
        public GameSettingsData gameSettings = new GameSettingsData();
        public WorldItemsData worldItems = new WorldItemsData();
        public List<MonsterData> monsters = new List<MonsterData>();
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
        public int level;
        public int experience;
        public int health;
        // Xóa mana vì nó được tính dựa trên Intelligence
        public Vector3 position = Vector3.zero;
        public int currency;
        public InventoryData inventory = new InventoryData();
        // Thêm các chỉ số base mới
        public int baseStrength;
        public int baseCritical;  // Đổi từ baseCharm
        public int baseIntelligence;
        public int baseStamina;
    }

    [Serializable]
    public class InventoryData
    {
        public List<ItemData> items = new List<ItemData>();
    }

    [Serializable]
    public class WorldItemsData
    {
        public List<WorldItemData> items = new List<WorldItemData>();
    }

    [Serializable]
    public class WorldItemData : ItemData
    {
        public Vector3 position;
        public bool isActive;
    }

    [Serializable]
    public class ItemData
    {
        public string id;
        public string itemName;
        public ItemType itemType;
        public int count = 1;

        // Specific properties based on item type
        public float defensePoints; // For armor items
        public float durability;    // For tools
        public int healthBonus;     // For food/consumables
        public int manaBonus;       // For potions
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
    public class WorldState
    {
        public string dayTime;
        public string weather;
        public int currentSceneIndex;
        public string sceneName;
    }

    [Serializable]
    public class GameSettingsData
    {
        public float volume;
        public string graphics;
    }

    [Serializable]
    public class MonsterData
    {
        public string id;
        public string type;
        public int level;
        public int health;
        public bool isDefeated;
        public Vector3 position;
        public string state; // IDLE, PATROL, PURSUE, ATTACK, SLEEP
    }



}
