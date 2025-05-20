// GameEvents.cs
using UnityEngine;
using System;

public enum GameEventType
{
    EnemyDefeated,    // Khi tiêu diệt quái thường
    BossDefeated,     // Khi đánh bại boss
    ItemCollected,    // Khi nhặt vật phẩm
    ItemCrafted,      // Khi chế tạo thành công
    ItemUpgraded,     // Khi nâng cấp thành công
    NpcTalked         // Khi trò chuyện với NPC
}

// GameEvents.cs
public static class GameEvents
{
    public static event Action<GameEventType, object> OnGameEvent;

    public static void Notify(GameEventType type, object data = null)
    {
        OnGameEvent?.Invoke(type, data);
    }
}