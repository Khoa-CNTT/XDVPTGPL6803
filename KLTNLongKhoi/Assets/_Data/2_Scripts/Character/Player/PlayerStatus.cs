using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    public class PlayerStatus : MonoBehaviour, IDamageable
    {   
        [Header("Sounds Effects")]
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip healSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip manaRestoreSound;
        [SerializeField] private AudioClip staminaRestoreSound;

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

            // Play damage sound
            if (damageSound != null)
            {
                AudioSource.PlayClipAtPoint(damageSound, transform.position);
            }

            if (playerStatsManager.CurrentHP <= 0)
            {
                isDead = true; 
                playerStatsManager.CurrentHP = 0;
                playerController.OnDead();
                ragdollAnimator.EnableRagdoll();

                // Play death sound
                if (deathSound != null)
                {
                    AudioSource.PlayClipAtPoint(deathSound, transform.position);
                }
            }
            else
            {
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void RestoreHealth(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(playerStatsManager.CurrentHP + amount, playerStatsManager.PlayerData.baseHP);
            playerStatsManager.CurrentHP = newHealth;

            // Play heal sound
            if (healSound != null)
            {
                AudioSource.PlayClipAtPoint(healSound, transform.position);
            }
        }

        // hồi mana
        public void RestoreMana(float amount)
        {
            if (isDead) return;

            float newMana = Mathf.Min(playerStatsManager.CurrentMP + amount, playerStatsManager.PlayerData.baseMP);
            playerStatsManager.CurrentMP = newMana;
            
            // Play mana restore sound
            if (manaRestoreSound != null)
            {
                AudioSource.PlayClipAtPoint(manaRestoreSound, transform.position);
            }
        }

        // hồi stamina
        public void RestoreStamina(float amount)
        {
            if (isDead) return;

            float newStamina = Mathf.Min(playerStatsManager.CurrentSP + amount, playerStatsManager.PlayerData.baseSP);
            playerStatsManager.CurrentSP = newStamina;
            
            // Play stamina restore sound
            if (staminaRestoreSound != null)
            {
                AudioSource.PlayClipAtPoint(staminaRestoreSound, transform.position);
            }
        }
    }
}