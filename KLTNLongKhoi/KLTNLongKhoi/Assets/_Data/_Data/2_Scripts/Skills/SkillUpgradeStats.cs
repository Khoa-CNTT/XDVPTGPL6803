using UnityEngine;

namespace KLTNLongKhoi
{
    [System.Serializable]
    public class SkillUpgradeStats
    {
        public int hpBonus;
        public int spBonus;
        public int mpBonus;
        public int physicalDamage;
        public int magicDamage;
        public int defensePoints;
        public int resistance;
        public float attackSpeed;
        public int healthRecovery;
        public int manaRecovery;
        public float criticalChance;
        public int cost;
        [Range(0, 1)] public float successRate = 1f;
    }
}