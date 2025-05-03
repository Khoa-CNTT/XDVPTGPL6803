using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    public class PlayerStatus : MonoBehaviour, IDamageable
    {
        private PlayerStatsManager playerStatsManager;
        private ThirdPersonController playerController;
        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        private bool isDead = false;
        private PauseManager pauseManager;
        private ThirdPersonController thirdPersonController;
        private GameManager gameManager;

        public bool IsDead() => isDead;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            playerController = GetComponent<ThirdPersonController>();
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            gameManager = FindFirstObjectByType<GameManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        public void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            if (isDead) return;

            // trừ cho giáp và sức chống chịu tính damage cuối
            float finalDamage = damage;
            playerStatsManager.CurrentHP -= finalDamage;

            if (playerStatsManager.CurrentHP <= 0)
            {
                playerStatsManager.CurrentHP = 0;
                Die(hitDirection);
            }
            else
            {
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void RestoreHealth(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(playerStatsManager.CurrentHP + amount, playerStatsManager.BaseHP);
            playerStatsManager.CurrentHP = newHealth;
        }

        // hồi mana
        public void RestoreMana(float amount)
        {
            if (isDead) return;

            float newMana = Mathf.Min(playerStatsManager.CurrentMP + amount, playerStatsManager.BaseMP);
            playerStatsManager.CurrentMP = newMana;
        }

        // hồi stamina
        public void RestoreStamina(float amount)
        {
            if (isDead) return;

            float newStamina = Mathf.Min(playerStatsManager.CurrentSP + amount, playerStatsManager.BaseSP);
            playerStatsManager.CurrentSP = newStamina;
        }

        private void Die(Vector3 hitDirection)
        {
            if (isDead) return;
            isDead = true;

            playerController.OnDead();
            ragdollAnimator.EnableRagdoll();
        }

    }
}
