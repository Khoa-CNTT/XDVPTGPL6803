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
            if (skillCooldownTimerQ > 0f)
            {
                skillCooldownTimerQ -= Time.fixedDeltaTime;
            }

            if (skillCooldownTimerE > 0f)
            {
                skillCooldownTimerE -= Time.fixedDeltaTime;
            }

            if (skillCooldownTimerC > 0f)
            {
                skillCooldownTimerC -= Time.fixedDeltaTime;
            }
        }

        private void ActivateSkillQ()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillQActive || isSkillEActive || isSkillCActive) return; 
            if (skillCooldownTimerQ > 0f) return;

            animator.SetBool("SkillQ", true);
            isSkillQActive = true;
            thirdPersonController.canMove = false;
            hitboxSkill.damage = playerStatsManager.GetPhysicsDamage;
        }

        private void ActivateSkillE()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillEActive || isSkillQActive || isSkillCActive) return; 
            if (skillCooldownTimerE > 0f) return;

            animator.SetBool("SkillE", true);
            isSkillEActive = true;
            thirdPersonController.canMove = false;
            hitboxSkill.damage = playerStatsManager.GetMagicDamage;
        }

        private void ActivateSkillC()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillCActive || isSkillQActive || isSkillEActive) return;
            if (skillCooldownTimerC > 0f) return;

            animator.SetBool("SkillC", true);
            isSkillCActive = true;
            thirdPersonController.canMove = false;
            hitboxSkill.damage = playerStatsManager.GetPhysicsDamage;
        }

        private void OnEndAnimation()
        {
            if (isSkillQActive)
            {
                isSkillQActive = false;
                thirdPersonController.canMove = true;
                animator.SetBool("SkillQ", false);
                skillCooldownTimerQ = skillCooldownQ;
            }
            else if (isSkillEActive)
            {
                isSkillEActive = false;
                thirdPersonController.canMove = true;
                animator.SetBool("SkillE", false);
                skillCooldownTimerE = skillCooldownE;
            }
            else if (isSkillCActive)
            {
                isSkillCActive = false;
                thirdPersonController.canMove = true;
                animator.SetBool("SkillC", false);
                skillCooldownTimerC = skillCooldownC;
            }
        }
    }
}