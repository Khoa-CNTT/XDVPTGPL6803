using KLTNLongKhoi;
using UnityEngine;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField] float skillCooldownQ = 5f, skillCooldownE = 5f, skillCooldownC = 5f;
        private float skillCooldownTimerQ = 0f, skillCooldownTimerE = 0f, skillCooldownTimerC = 0f;
        private bool isSkillQActive = false, isSkillEActive = false, isSkillCActive = false;

        private ActorHitbox hitboxSkill;
        private PlayerStatsManager playerStatsManager;
        private PauseManager pauseManager;
        private StarterAssetsInputs starterAssetsInputs;
        private ThirdPersonController thirdPersonController;
        private Animator animator;
        private CharacterAnimationEvents characterAnimationEvents;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            animator = GetComponentInChildren<Animator>();
            hitboxSkill = GetComponentInChildren<ActorHitbox>();
            characterAnimationEvents = GetComponentInChildren<CharacterAnimationEvents>();
        }

        private void OnEnable()
        {
            starterAssetsInputs.SkillQ += ActivateSkillQ;
            starterAssetsInputs.SkillE += ActivateSkillE;
            starterAssetsInputs.SkillC += ActivateSkillC;
            characterAnimationEvents.onEndAnimation += OnEndAnimation;
        }

        private void OnDisable()
        {
            starterAssetsInputs.SkillQ -= ActivateSkillQ;
            starterAssetsInputs.SkillE -= ActivateSkillE;
            starterAssetsInputs.SkillC -= ActivateSkillC;
            characterAnimationEvents.onEndAnimation -= OnEndAnimation;
        }

        private void FixedUpdate()
        {
            if (pauseManager.IsPaused) return;
            UpdateSkillCooldowns();
        }

        private void UpdateSkillCooldowns()
        {
            if (skillCooldownTimerQ > 0f) skillCooldownTimerQ -= Time.fixedDeltaTime;
            if (skillCooldownTimerE > 0f) skillCooldownTimerE -= Time.fixedDeltaTime;
            if (skillCooldownTimerC > 0f) skillCooldownTimerC -= Time.fixedDeltaTime;
        }

        private void ActivateSkillQ()
        {
            if (!CanUseSkill(skillCooldownTimerQ)) return;

            isSkillQActive = true;
            thirdPersonController.canMove = false;
            animator.SetBool("SkillQ", true);

            hitboxSkill.damage = playerStatsManager.GetPhysicsDamage;
            hitboxSkill.attacker = this.transform;
        }

        private void ActivateSkillE()
        {
            if (!CanUseSkill(skillCooldownTimerE)) return;

            isSkillEActive = true;
            thirdPersonController.canMove = false;
            animator.SetBool("SkillE", true);

            hitboxSkill.damage = playerStatsManager.GetMagicDamage;
            hitboxSkill.attacker = this.transform;
        }

        private void ActivateSkillC()
        {
            if (!CanUseSkill(skillCooldownTimerC)) return;

            isSkillCActive = true;
            thirdPersonController.canMove = false;
            animator.SetBool("SkillC", true);

            hitboxSkill.damage = playerStatsManager.GetPhysicsDamage;
            hitboxSkill.attacker = this.transform;
        }

        private bool CanUseSkill(float cooldown)
        {
            return thirdPersonController.canMove &&
                   !isSkillQActive && !isSkillEActive && !isSkillCActive &&
                   cooldown <= 0f;
        }

        private void OnEndAnimation()
        {
            if (isSkillQActive)
            {
                isSkillQActive = false;
                skillCooldownTimerQ = skillCooldownQ;
                animator.SetBool("SkillQ", false);
            }
            else if (isSkillEActive)
            {
                isSkillEActive = false;
                skillCooldownTimerE = skillCooldownE;
                animator.SetBool("SkillE", false);
            }
            else if (isSkillCActive)
            {
                isSkillCActive = false;
                skillCooldownTimerC = skillCooldownC;
                animator.SetBool("SkillC", false);
            }

            thirdPersonController.canMove = true;
        }
    }
}
