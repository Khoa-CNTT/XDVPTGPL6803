using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemInventory", menuName = "Inventory/itemInventory")]
public class ItemInventory : ScriptableObject
{
    public int Id;
    public string ItemName;
    public int Value;
    public int MaxStack = 3; // Số lượng tối đa có thể stack
    public Sprite Image;
    public enum StatType { None, PlayerStats, AffectionNpc }
    public StatType ItemTag; // Đổi từ string Tag -> StatType ItemTag
    public enum ScriptType { None, ModifyStatffect, AffectionNpc }
    public ScriptType ScriptEffect;
    [TextArea]
    public string Describe;
    public List<ItemEffect> StatsValue; // Danh sách hiệu ứng của item
    public Affection AffectionNpc;
}

[System.Serializable]
public class ItemEffect
{
    // public PlayerStats.StatType statType; // Chỉ số bị ảnh hưởng (HP, Money, Strength,...)
    public int value; // Giá trị tăng/giảm
}
[System.Serializable]
public class Affection
{
    public int AffectionValue;
}