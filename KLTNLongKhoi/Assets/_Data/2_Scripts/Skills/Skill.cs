using UnityEngine;
using System;

namespace KLTNLongKhoi
{
    public class Skill
    {
        public SkillData Data { get; private set; }
        public float CurrentCooldown { get; private set; }
        public bool IsReady => CurrentCooldown <= 0;
        public bool IsUnlocked { get; private set; }
        
        private Action<Vector3> onSkillCast;
        
        public Skill(SkillData data)
        {
            Data = data;
            CurrentCooldown = 0;
            IsUnlocked = false;
        }

        public void Update()
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= Time.deltaTime;
            }
        }

        public bool TryCast(PlayerStatsManager statsManager, Vector3 targetPosition)
        {
            if (!CanCast(statsManager))
                return false;

            // Tiêu thụ mana
            statsManager.ConsumeMana(Data.manaCost);
            
            // Start cooldown
            CurrentCooldown = Data.cooldownTime;
            
            // Trigger effects
            onSkillCast?.Invoke(targetPosition);
            
            return true;
        }

        private bool CanCast(PlayerStatsManager statsManager)
        {
            if (!IsUnlocked) return false;
            if (!IsReady) return false;
            if (statsManager.Level < Data.levelRequired) return false;
            if (!statsManager.HasEnoughMana(Data.manaCost)) return false;
            return true;
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public void SetCastCallback(Action<Vector3> callback)
        {
            onSkillCast = callback;
        }
    }
}
