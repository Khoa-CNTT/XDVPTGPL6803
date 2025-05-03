using UnityEngine;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField] float skillCooldownQ = 5f, skillCooldownE = 5f, skillCooldownC = 5f;
        private float skillCooldownTimerQ = 0f, skillCooldownTimerE = 0f, skillCooldownTimerC = 0f;
        private bool isSkillQActive = false, isSkillEActive = false, isSkillCActive = false;
        private PlayerStatsManager playerStatsManager;
        private PauseManager pauseManager;
        private StarterAssetsInputs starterAssetsInputs;
        private PlayerAnimationEvents playerAnimationEvents;

        private void Awake()
        {
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            playerAnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        }

        private void Start()
        { 
            playerAnimationEvents.onEndAnimation += OnEndAnimation;
        }

        private void OnEnable()
        {
            starterAssetsInputs.SkillQ += ActivateSkillQ;
            starterAssetsInputs.SkillE += ActivateSkillE;
            starterAssetsInputs.SkillC += ActivateSkillC;
        }

        private void OnDisable()
        {
            starterAssetsInputs.SkillQ -= ActivateSkillQ;
            starterAssetsInputs.SkillE -= ActivateSkillE;
            starterAssetsInputs.SkillC -= ActivateSkillC;
        }

        private void FixedUpdate()
        {
            if (pauseManager.IsPaused) return;

            UpdateSkillCooldowns();
        }

        private void UpdateSkillCooldowns()
        {
            if (isSkillQActive)
            {
                skillCooldownTimerQ -= Time.deltaTime;
                if (skillCooldownTimerQ <= 0f)
                {
                    isSkillQActive = false;
                    skillCooldownTimerQ = skillCooldownQ;
                }
            }

            if (isSkillEActive)
            {
                skillCooldownTimerE -= Time.deltaTime;
                if (skillCooldownTimerE <= 0f)
                {
                    isSkillEActive = false;
                    skillCooldownTimerE = skillCooldownE;
                }
            }

            if (isSkillCActive)
            {
                skillCooldownTimerC -= Time.deltaTime;
                if (skillCooldownTimerC <= 0f)
                {
                    isSkillCActive = false;
                    skillCooldownTimerC = skillCooldownC;
                }
            }
        }

        private void OnEndAnimation()
        {
            if (isSkillQActive)
            {
                isSkillQActive = false;
            }
            else if (isSkillEActive)
            {
                isSkillEActive = false;
            }
            else if (isSkillCActive)
            {
                isSkillCActive = false;
            }
        }

        private void ActivateSkillQ()
        {
            Debug.Log("Skill Q");
            
        }

        private void ActivateSkillE()
        {
            Debug.Log("Skill E");
        }

        private void ActivateSkillC()
        {
            Debug.Log("Skill C");
        }
    }
}