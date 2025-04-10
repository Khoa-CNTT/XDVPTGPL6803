using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill Data")]
    public class SkillData : ScriptableObject
    {
        [Header("Basic Info")]
        public string skillId;
        public string skillName;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;
        
        [Header("Requirements")]
        public int levelRequired;
        public float manaCost;
        public float cooldownTime;
        
        [Header("Skill Properties")]
        public SkillType skillType;
        public TargetType targetType;
        public float range;
        public float damage;
        public float duration;
        
        [Header("Effects")]
        public GameObject skillEffectPrefab;
        public AudioClip skillSound;
        public ParticleSystem castEffect;
    }

    public enum SkillType
    {
        Active,     // Skills that need to be activated
        Passive,    // Always-on effects
        Toggle      // Can be turned on/off
    }

    public enum TargetType
    {
        Self,
        SingleTarget,
        AOE,
        Directional
    }
}