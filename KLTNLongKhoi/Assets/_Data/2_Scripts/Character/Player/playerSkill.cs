using UnityEngine;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField] float skillCooldownQ = 5f, skillCooldownE = 5f, skillCooldownC = 5f;
        private float skillCooldownTimerQ = 0f, skillCooldownTimerE = 0f, skillCooldownTimerC = 0f;
        private bool isSkillQActive = false, isSkillEActive = false, isSkillCActive = false;
        private ActorHitbox hitboxSkillQ, hitboxSkillE, hitboxSkillC;
        private PlayerStatsManager playerStatsManager;
        private PauseManager pauseManager;
        private StarterAssetsInputs starterAssetsInputs;
        private ThirdPersonController thirdPersonController;
        private Animator animator;

        private void Awake()
        {
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
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

        private void ActivateSkillQ()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillQActive) return;
            Debug.Log("Skill Q");

            animator.SetTrigger("SkillQ");
            isSkillQActive = true;
            thirdPersonController.canMove = false;
            Invoke("OnEndSkillQ", 1f);
        }

        private void OnEndSkillQ()
        {
            isSkillQActive = false;
            thirdPersonController.canMove = true;
        }

        private void ActivateSkillE()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillEActive) return;
            Debug.Log("Skill E");

            animator.SetTrigger("SkillE");
            isSkillEActive = true;
            thirdPersonController.canMove = false;
            Invoke("OnEndSkillE", 1f);
        }

        private void OnEndSkillE()
        {
            isSkillEActive = false;
            thirdPersonController.canMove = true;
        }

        private void ActivateSkillC()
        {
            if (thirdPersonController.canMove == false) return;
            if (isSkillCActive) return;
            Debug.Log("Skill C");

            animator.SetTrigger("SkillC");
            isSkillCActive = true;
            thirdPersonController.canMove = false;
            Invoke("OnEndSkillC", 1f);
        }

        private void OnEndSkillC()
        {
            isSkillCActive = false;
            thirdPersonController.canMove = true;
        }
    }
}