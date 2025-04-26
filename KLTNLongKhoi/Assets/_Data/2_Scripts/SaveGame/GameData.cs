using System;
using System.Collections.Generic;
using System.Linq;
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
        public Vector3 position = Vector3.zero;
        public int currency;
        public int diamonds;
        public InventoryData inventory = new InventoryData();
        public int baseStrength;
        public int baseCritical;
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
        // Audio Settings
        public float masterVolume = 1f;
        public float musicVolume = 1f;
        public float sfxVolume = 1f;

        // Graphics Settings
        public int graphics = 1;
        public string resolution = "1920x1080";
        public int targetFrameRate = 60;
        public float brightness = 1f; 

        // Gameplay Settings
        public float mouseSensitivity = 1f;
        public bool invertYAxis = false;
        public string language = "English";
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

    [Serializable]
    public class ActorStats
    {
        public string actorName;
        public string id;
        public string state;
    }



    [System.Serializable]
    public class ItemStats
    {
        // Combat stats
        public int physicalDamage;
        public int magicDamage;
        public int armor;
        public int resistance;
        [Range(0, 1)] public float attackSpeed;

        // Bonus stats
        public int healthBonus;
        public int manaBonus;
        public int healthRecovery;
        public int manaRecovery;

        // Hidden stats
        public float criticalChance;
        public float dodgeChance;
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

    public enum WeaponType
    {
        Dagger,
        Spear,
        Sword,
        Shield
    }

    public enum ArmorType
    {
        Cloak,      // Áo choàng
        BronzeRing, // Vòng đồng
        DragonHelm, // Mũ rồng
        Loincloth,  // Khố
        DragonBlood // Long Huyết
    }

    public enum ConsumableType
    {
        HealthPotion, // Bình máu
        ManaPotion,
        BuffPotion
    }

    public enum ResourceType
    {
        Stone,      // Đá
        Iron,       // Sắt
        Wood,       // Gỗ
        Coral,      // San hô
        Pearl,      // Ngọc trai
        FishScale,  // Da Ngư Tinh
        FoxFur,     // Lông Hồ Tinh
        FairyWater, // Nước tiên
        Rope,       // Dây thừng
        Quartz,     // Đá thạch anh
        Gemstone,   // Đá quý
        SpiderSilk  // Tơ nhện
    }
}
