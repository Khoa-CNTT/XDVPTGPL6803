#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using KLTNLongKhoi;

public class ItemCreatorWindow : EditorWindow
{
    private string itemName = "New Item";
    private ItemType itemType = ItemType.Weapon;
    
    // Weapon specific
    private WeaponType weaponType = WeaponType.Dagger;
    private int weaponLevel = 1;
    
    // Armor specific
    private ArmorType armorType = ArmorType.Cloak;
    private int armorLevel = 0;
    
    // Consumable specific
    private ConsumableType consumableType = ConsumableType.HealthPotion;
    private int consumableLevel = 1;
    
    // Resource specific
    private ResourceType resourceType = ResourceType.Stone;
    
    [MenuItem("Game Tools/Item Creator")]
    public static void ShowWindow()
    {
        GetWindow<ItemCreatorWindow>("Item Creator");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Create New Item", EditorStyles.boldLabel);
        
        itemName = EditorGUILayout.TextField("Item Name", itemName);
        itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);
        
        switch (itemType)
        {
            case ItemType.Weapon:
                weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponType);
                weaponLevel = EditorGUILayout.IntSlider("Level", weaponLevel, 1, 3);
                break;
                
            case ItemType.Armor:
                armorType = (ArmorType)EditorGUILayout.EnumPopup("Armor Type", armorType);
                if (armorType == ArmorType.DragonBlood)
                {
                    armorLevel = EditorGUILayout.IntSlider("Level", armorLevel, 1, 2);
                }
                break;
                
            case ItemType.Consumable:
                consumableType = (ConsumableType)EditorGUILayout.EnumPopup("Consumable Type", consumableType);
                if (consumableType == ConsumableType.HealthPotion)
                {
                    consumableLevel = EditorGUILayout.IntSlider("Level", consumableLevel, 1, 3);
                }
                break;
                
            case ItemType.Resource:
                resourceType = (ResourceType)EditorGUILayout.EnumPopup("Resource Type", resourceType);
                break;
        }
        
        if (GUILayout.Button("Create Item"))
        {
            CreateItem();
        }
    }
    
    private void CreateItem()
    {
        string basePath = "Assets/_Data/2_Scripts/Items/SO/";
        string assetPath = "";
        ScriptableObject newItem = null;
        
        switch (itemType)
        {
            case ItemType.Weapon:
                basePath += "Weapons/" + weaponType.ToString() + "/";
                newItem = CreateInstance<WeaponDataSO>();
                SetupWeapon((WeaponDataSO)newItem);
                assetPath = basePath + itemName.Replace(" ", "_") + ".asset";
                break;
                
            case ItemType.Armor:
                basePath += "Armors/" + armorType.ToString() + "/";
                newItem = CreateInstance<ArmorDataSO>();
                SetupArmor((ArmorDataSO)newItem);
                assetPath = basePath + itemName.Replace(" ", "_") + ".asset";
                break;
                
            case ItemType.Consumable:
                basePath += "Consumables/" + consumableType.ToString() + "/";
                newItem = CreateInstance<ConsumableDataSO>();
                SetupConsumable((ConsumableDataSO)newItem);
                assetPath = basePath + itemName.Replace(" ", "_") + ".asset";
                break;
                
            case ItemType.Resource:
                basePath += "Resources/" + resourceType.ToString() + "/";
                newItem = CreateInstance<ResourceDataSO>();
                SetupResource((ResourceDataSO)newItem);
                assetPath = basePath + itemName.Replace(" ", "_") + ".asset";
                break;
        }
        
        if (!AssetDatabase.IsValidFolder(basePath))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + basePath.Substring("Assets".Length));
        }
        
        AssetDatabase.CreateAsset(newItem, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newItem;
    }
    
    private void SetupWeapon(WeaponDataSO weapon)
    {
        weapon.itemName = itemName;
        weapon.weaponType = weaponType;
        weapon.level = weaponLevel;
        weapon.itemID = GenerateWeaponID(weaponType, weaponLevel);
        
        // Set default stats based on weapon type and level
        weapon.stats = new ItemStats();
        
        switch (weaponType)
        {
            case WeaponType.Dagger:
                if (weaponLevel == 1)
                {
                    weapon.stats.physicalDamage = 15;
                    weapon.stats.attackSpeed = 0.35f;
                }
                break;
                
            // Add other weapon types here...
        }
    }
    
    private string GenerateWeaponID(WeaponType type, int level)
    {
        return $"WEAPON_{type.ToString().ToUpper()}_LV{level}";
    }
    
    private void SetupArmor(ArmorDataSO armor)
    {
        armor.itemName = itemName;
        armor.armorType = armorType;
        armor.level = armorLevel;
        armor.itemID = GenerateArmorID(armorType, armorLevel);
        
        // Set default stats
        armor.stats = new ItemStats();
        
        switch (armorType)
        {
            case ArmorType.Cloak:
            case ArmorType.BronzeRing:
            case ArmorType.DragonHelm:
            case ArmorType.Loincloth:
                armor.stats.healthBonus = 1000;
                armor.stats.manaBonus = 200;
                break;
                
            case ArmorType.DragonBlood:
                if (armorLevel == 1) // Sơ cấp
                {
                    armor.stats.healthBonus = 1000;
                    armor.stats.manaBonus = 200;
                }
                else if (armorLevel == 2) // Trung cấp
                {
                    armor.stats.healthBonus = 1500;
                    armor.stats.manaBonus = 500;
                }
                break;
        }
    }
    
    private string GenerateArmorID(ArmorType type, int level)
    {
        if (type == ArmorType.DragonBlood)
        {
            return $"ARMOR_{type.ToString().ToUpper()}_LV{level}";
        }
        return $"ARMOR_{type.ToString().ToUpper()}";
    }
    
    private void SetupConsumable(ConsumableDataSO consumable)
    {
        consumable.itemName = itemName;
        consumable.consumableType = consumableType;
        consumable.level = consumableLevel;
        consumable.itemID = GenerateConsumableID(consumableType, consumableLevel);
        
        // Set default stats
        consumable.stats = new ItemStats();
        
        if (consumableType == ConsumableType.HealthPotion)
        {
            switch (consumableLevel)
            {
                case 1: consumable.stats.healthRecovery = 80; break;
                case 2: consumable.stats.healthRecovery = 250; break;
                case 3: consumable.stats.healthRecovery = 1500; break;
            }
        }
    }
    
    private string GenerateConsumableID(ConsumableType type, int level)
    {
        if (type == ConsumableType.HealthPotion)
        {
            return $"POTION_{type.ToString().ToUpper()}_LV{level}";
        }
        return $"POTION_{type.ToString().ToUpper()}";
    }
    
    private void SetupResource(ResourceDataSO resource)
    {
        resource.itemName = itemName;
        resource.resourceType = resourceType;
        resource.itemID = $"RESOURCE_{resourceType.ToString().ToUpper()}";
    }
}
#endif
