using UnityEngine;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private List<SkillData> startingSkills;
        
        private Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
        private PlayerStatsManager statsManager;

        private void Awake()
        {
            statsManager = GetComponent<PlayerStatsManager>();
            InitializeSkills();
        }

        private void Update()
        {
            foreach (var skill in skills.Values)
            {
                skill.Update();
            }
        }

        private void InitializeSkills()
        {
            foreach (var skillData in startingSkills)
            {
                AddSkill(skillData);
            }
        }

        public void AddSkill(SkillData skillData)
        {
            if (!skills.ContainsKey(skillData.skillId))
            {
                var skill = new Skill(skillData);
                skills.Add(skillData.skillId, skill);
                SetupSkillEffects(skill);
            }
        }

        private void SetupSkillEffects(Skill skill)
        {
            skill.SetCastCallback((targetPosition) =>
            {
                // Spawn visual effects
                if (skill.Data.skillEffectPrefab != null)
                {
                    Instantiate(skill.Data.skillEffectPrefab, targetPosition, Quaternion.identity);
                }

                // Play sound
                if (skill.Data.skillSound != null)
                {
                    AudioSource.PlayClipAtPoint(skill.Data.skillSound, transform.position);
                }

                // Spawn particles
                if (skill.Data.castEffect != null)
                {
                    var effect = Instantiate(skill.Data.castEffect, transform.position, Quaternion.identity);
                    effect.Play();
                }

                // Apply damage or effects based on skill type
                ApplySkillEffects(skill, targetPosition);
            });
        }

        private void ApplySkillEffects(Skill skill, Vector3 targetPosition)
        {
            switch (skill.Data.targetType)
            {
                case TargetType.SingleTarget:
                    // Implement raycast or sphere cast to find target
                    break;
                    
                case TargetType.AOE:
                    // Find all targets in range
                    var hits = Physics.SphereCastAll(targetPosition, skill.Data.range, Vector3.up);
                    foreach (var hit in hits)
                    {
                        var damageable = hit.collider.GetComponent<IDamageable>();
                        damageable?.TakeDamage(skill.Data.damage, (hit.point - transform.position).normalized);
                    }
                    break;
                    
                // Add other cases as needed
            }
        }

        public bool TryCastSkill(string skillId, Vector3 targetPosition)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill.TryCast(statsManager, targetPosition);
            }
            return false;
        }

        public void UnlockSkill(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                skill.Unlock();
            }
        }

        public float GetSkillCooldown(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill.CurrentCooldown;
            }
            return 0f;
        }

        public bool IsSkillReady(string skillId)
        {
            if (skills.TryGetValue(skillId, out Skill skill))
            {
                return skill.IsReady;
            }
            return false;
        }
    }
}