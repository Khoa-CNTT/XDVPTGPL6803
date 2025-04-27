using UnityEngine;
using System;

namespace KLTNLongKhoi
{
    public class PlayerSkill : MonoBehaviour
    {
        public SkillData Data { get; private set; }
        public float CurrentCooldown { get; private set; }
        public bool IsReady => CurrentCooldown <= 0;
        public bool IsUnlocked { get; private set; }

        private Action<Vector3> onSkillCast;

        public PlayerSkill(SkillData data)
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

        // Thực hiện cast skill, trả về true nếu cast thành công, false nếu không đủ điều kiện cast
        public bool OnUseSkill(PlayerStatsManager statsManager, Vector3 targetPosition)
        {

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
