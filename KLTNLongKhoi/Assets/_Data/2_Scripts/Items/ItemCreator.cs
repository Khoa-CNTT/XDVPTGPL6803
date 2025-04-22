#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace KLTNLongKhoi.Editor
{
    public class ItemCreator : EditorWindow
    {
        private string itemName = "New Item";
        private ItemType itemType = ItemType.Weapon;
        private WeaponType weaponType = WeaponType.Dagger;
        private int level = 1;

        [MenuItem("Tools/Item Creator")]
        public static void ShowWindow()
        {
            GetWindow<ItemCreator>("Item Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create New Item", EditorStyles.boldLabel);

            itemName = EditorGUILayout.TextField("Item Name", itemName);
            itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);

            if (itemType == ItemType.Weapon)
            {
                weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponType);
                level = EditorGUILayout.IntField("Level", level);
            }

            if (GUILayout.Button("Create Item"))
            {
                CreateItem();
            }
        }

        private void CreateItem()
        {
            string path = "Assets/Items/";
            string fileName = itemName.Replace(" ", "_");

            switch (itemType)
            {
                case ItemType.Weapon:
                    var weapon = CreateInstance<WeaponDataSO>();
                    weapon.itemName = itemName;
                    weapon.weaponType = weaponType;
                    weapon.level = level;
                    weapon.itemID = $"WEAPON_{weaponType.ToString().ToUpper()}_LV{level}";
                    
                    // Set default stats based on weapon type and level
                    SetWeaponStats(weapon);
                    
                    path += $"Weapons/{weaponType}/";
                    AssetDatabase.CreateAsset(weapon, path + fileName + ".asset");
                    break;
                    
                // Add cases for other item types here
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        private void SetWeaponStats(WeaponDataSO weapon)
        {
            weapon.stats = new ItemStats();
            
            switch (weapon.weaponType)
            {
                case WeaponType.Dagger:
                    if (weapon.level == 1)
                    {
                        weapon.stats.physicalDamage = 15;
                        weapon.stats.attackSpeed = 0.35f;
                    }
                    break;
                    
                case WeaponType.Spear:
                    if (weapon.level == 1)
                    {
                        weapon.stats.physicalDamage = 25;
                        weapon.stats.resistance = 10;
                        weapon.stats.attackSpeed = 0.30f;
                    }
                    else if (weapon.level == 2)
                    {
                        weapon.stats.physicalDamage = 120;
                        weapon.stats.magicDamage = 30;
                        weapon.stats.resistance = 35;
                        weapon.stats.attackSpeed = 0.28f;
                    }
                    else if (weapon.level == 3)
                    {
                        weapon.stats.physicalDamage = 250;
                        weapon.stats.magicDamage = 60;
                        weapon.stats.resistance = 100;
                        weapon.stats.attackSpeed = 0.40f;
                        weapon.stats.healthRecovery = 10;
                    }
                    break;
                    
                case WeaponType.Sword:
                    if (weapon.level == 1)
                    {
                        weapon.stats.physicalDamage = 45;
                        weapon.stats.resistance = 25;
                        weapon.stats.attackSpeed = 0.25f;
                    }
                    else if (weapon.level == 2)
                    {
                        weapon.stats.physicalDamage = 85;
                        weapon.stats.resistance = 35;
                        weapon.stats.attackSpeed = 0.28f;
                    }
                    else if (weapon.level == 3)
                    {
                        weapon.stats.physicalDamage = 100;
                        weapon.stats.resistance = 40;
                        weapon.stats.attackSpeed = 0.40f;
                    }
                    break;
                    
                case WeaponType.Shield:
                    if (weapon.level == 1)
                    {
                        weapon.stats.armor = 300;
                        weapon.stats.attackSpeed = 0.60f;
                        weapon.stats.healthRecovery = 10;
                        weapon.stats.resistance = 350;
                    }
                    else if (weapon.level == 2)
                    {
                        weapon.stats.armor = 500;
                        weapon.stats.attackSpeed = 0.65f;
                        weapon.stats.healthRecovery = 25;
                        weapon.stats.resistance = 600;
                    }
                    else if (weapon.level == 3)
                    {
                        weapon.stats.armor = 1000;
                        weapon.stats.attackSpeed = 0.65f;
                        weapon.stats.healthRecovery = 40;
                        weapon.stats.resistance = 1000;
                    }
                    break;
            }
        }
    }
}
#endif