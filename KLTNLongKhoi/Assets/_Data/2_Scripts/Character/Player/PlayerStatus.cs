using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    public class PlayerStatus : MonoBehaviour, IDamageable
    {
        private PlayerStatsManager statsManager;
        private ThirdPersonController playerController;
        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        private bool isDead = false;
        private GameManager gameManager;
        private PauseManager pauseManager;

        private void Awake()
        {
            statsManager = FindFirstObjectByType<PlayerStatsManager>();
            playerController = GetComponent<ThirdPersonController>();
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            gameManager = FindFirstObjectByType<GameManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        public void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            if (isDead) return;

            float finalDamage = statsManager.CalculateFinalDamage(damage);
            statsManager.CurrentHealth -= finalDamage;

            if (statsManager.CurrentHealth <= 0)
            {
                statsManager.CurrentHealth = 0;
                Die(hitDirection);
            }
            else
            {
                playerController?.TakeHit(hitDirection, finalDamage);
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void Heal(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(statsManager.CurrentHealth + amount, statsManager.MaxHealth);
            statsManager.CurrentHealth = newHealth;
        }

        private void Die(Vector3 hitDirection)
        {
            if (isDead) return;
            isDead = true;

            playerController.Die();
            ragdollAnimator.EnableRagdoll();
            gameManager.GameOver();
        }

        public void RespawnToLastPoint()
        {
            if (!isDead) return;

            transform.position = statsManager.LastCheckpoint;
            isDead = false;

            ragdollAnimator.DisableRagdoll();
            pauseManager.ResumeGame();
        }

        // Public getters
        public float GetCurrentHealth() => statsManager.CurrentHealth;
        public float GetMaxHealth() => statsManager.MaxHealth;
        public bool IsDead() => isDead;
    }
}
